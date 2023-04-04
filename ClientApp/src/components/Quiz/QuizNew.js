import React, { useEffect, useState } from "react";
import { useNavigate, useParams, useResolvedPath } from "react-router-dom";
import authService from "../api-authorization/AuthorizeService";
import { TEACHER_ROLE, authenticate } from "../../roles";
import { Button, Card, FormControl, InputLabel, MenuItem, Pagination, Select, Stack, TextField } from "@mui/material";
import QueryNew from "./QueryNew";

export default function QuizNew(){
    const navigate = useNavigate()
    const id = useParams();

    const [state, setState] = useState( { quiz: undefined, isReady: false, isPosting: false })
    const [page, setPage] = useState(-1)
    const [user, setUser] = useState(0)

    useEffect(() => {})

    useEffect(() => {
        async function loadUser() {
            const u = await authService.getUser()
            setUser(u)
        }

        if (user === 0)
            loadUser()
        console.log(user)
        if (!authenticate(user.role, TEACHER_ROLE))
            navigate(-1)
    }, [user])

    useEffect(() => {
        if (state.isReady) return;
        const quizJson = localStorage.getItem(`quiz${id}`);
        const quiz = JSON.parse(quizJson)
        setState(
            {
                ...state,
                isReady: true,
                quiz : !!quiz? quiz : {
                    name: '',
                    description: '',
                    duration: '',
                    qType: 0,
                    count: 0,
                    lessonId: id,
                    questions: []
                }
            }
        )
    }, [state.isReady])

    useEffect(() => {
        console.log(state.quiz)
        if (!!state.quiz){ 
            localStorage.setItem(`quiz${id}`, JSON.stringify(state.quiz))
        }
    }, [state.quiz])

    function SetQuestion(question){
        setState((prev) => {
            const queries = prev.quiz.questions;
            queries[page] = question

            const newState = {
                ...prev, 
                quiz: {
                    ...prev.quiz,
                    questions: queries
                }
            }

            return newState;
        })
    }

    function RemoveQuestion(n){
        setState((prev) => {
            const newState = {
                ...prev,
                quiz: {
                    ...prev.quiz,
                    questions: prev.quiz.questions.splice(n, 1)
                }
            }

            if (page >= state.quiz.questions.length)
                setPage(state.quiz.questions.length - 1)
            return newState;
        })
    }

    if (!state.isReady)
        return <p><i>Loading...</i></p>

    if (!!state.quiz){ 
        return (
        <Stack direction={'column'} spacing={1}>
            <TextField label='Name' value={state.name} 
                onChange={(e) => setState((prev) => { 
                    return {...prev, name: e.target.value}
                })}
            />
            <TextField label='Description' value={state.description}
                onChange={(e) => setState((prev) => { 
                    return {...prev, description: e.target.value}
                })}
            />

            <FormControl>
                <InputLabel id="select-test-type">Select test type</InputLabel>
                <Select
                    labelId="select-test-type"
                    value={state.quiz.qType}
                    label="Condition"
                    onChange={e => {
                    setState((prev) => (
                        {
                            ...prev,
                            quiz: 
                            {
                                ...prev.quiz,
                                qType: e.target.value
                            }
                        }
                    ))}}
                >
                    <MenuItem value={0}>Normal test</MenuItem>
                    <MenuItem value={1}>Hardcore (one attempt)</MenuItem>
                    <MenuItem value={2}>Key test</MenuItem>
                    <MenuItem value={4}>Key-Hardcore test</MenuItem>
                </Select>
            </FormControl>
            
            <TextField label='Duration (min)' value={state.duration} type="number"
                onChange={(e) => setState((prev) => { 
                    return {...prev, duration: e.target.value}
                })}
            />
            <TextField label='Count' value={state.count} type="number" 
                onChange={(e) => setState((prev) => { 
                    return {...prev, count: e.target.value}
                })}
            />
        
            {page >= 0 && <Card key={page}>
                <QueryNew 
                    page={page}
                    query={state.quiz.questions[page-1]}
                    onDone={(question) => SetQuestion(question)}
                />
            </Card>}
            
            <Stack direction={'row'} >
                <Button label='Save' color='success' onClick={() => console.log(state)} />
            </Stack>

            <Stack direction={'row'} spacing={3} justifyContent={'space-evenly'} >
                <Button label='Add' color="primary"
                    onClick={() => setState({
                        ...state,
                        quiz: {
                            ...state.quiz,
                            questions: [...state.quiz.questions, { content: '', count: 0, replies: undefined }]
                        }
                    })}
                >Add question</Button>
                <Button color="error" disabled={page <= 0} 
                    onClick={() => {
                        const quests = state.quiz.questions;
                        quests.splice(page-1, 1)
                        setState({
                            ...state,
                            quiz: {
                                ...state.quiz,
                                questions: quests
                            }
                        })

                        if (page > quests.length) 
                            setPage(quests.length)
                    }}
                >Remove question</Button>
            </Stack>

            {state.quiz.questions.length > 0 && <Pagination 
                count={state.quiz.questions.length}
                page={page}
                onChange={(e, v) => setPage(v)}
            />}
            
        </Stack>)
    }
}