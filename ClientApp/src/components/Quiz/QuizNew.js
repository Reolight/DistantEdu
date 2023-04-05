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

    useEffect(() => {console.log(state)}, [state.quiz])

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
                    duration: 0,
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
            queries[page - 1] = question

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
            <TextField label='Name' value={state.quiz.name} 
                error={state.quiz.name.length < 3}
                helperText={state.quiz.name.length < 3&& 'More than 2 symbols'}
                onChange={(e) => setState((prev) => {
                    return {...prev, quiz: { ...prev.quiz, name: e.target.value}}
                })}
            />
            <TextField label='Description' value={state.quiz.description}
                error={state.quiz.description < 10}
                helperText={state.quiz.description.length < 10 && 'More than 10 symbols'}
                onChange={(e) => setState((prev) => { 
                    return {...prev, quiz: {...prev.quiz, description: e.target.value}}
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
            
            <TextField label='Duration (min)' value={state.quiz.duration} type="number"
                onChange={(e) => setState((prev) => { 
                    return {...prev, quiz: {...state.quiz, duration: e.target.value}}
                })}
            />
            <TextField label='Count' value={state.quiz.count} type="number" 
                onChange={(e) => setState((prev) => { 
                    return {...prev, quiz: {...state.quiz, count: e.target.value}}
                })}
            />
        
            {page >= 0 && <Card key={page}>
                <QueryNew 
                    page={page-1}
                    query={state.quiz.questions[page-1]}
                    onDone={(question) => SetQuestion(question)}
                />
            </Card>}
            
            <Stack direction={'row'} >
                <Button label='Save' color='success' onClick={() => console.log(state)} />
            </Stack>

            <Stack direction={'row'} spacing={3} justifyContent={'space-evenly'} >
                <Button label='Add' color="primary"
                    onClick={() => {setState({
                        ...state,
                        quiz: {
                            ...state.quiz,
                            questions: [...state.quiz.questions, { content: '', count: 0, replies: [] }]
                            }
                        })

                        setPage(state.quiz.questions.length+1)
                    }}
                >Add question</Button>
                <Button color="error" disabled={page <= 0} 
                    onClick={() => {
                        const quests = state.quiz.questions;
                        if (!!quests[page-1] &&
                            (quests[page-1].content !== '' ||
                            quests[page-1].replies.length > 0))
                        {
                            const conf = window.confirm('Are you realy want to delete not empty question?')
                            if (!conf)
                                return
                        }
                        
                        quests.splice(page-1, 1)
                        setState({
                            ...state,
                            quiz: {
                                ...state.quiz,
                                questions: quests
                            }
                        })

                        setPage((prev) => prev-1)
                    }}
                >Remove question</Button>
            </Stack>
            
            <Stack justifyItems={'center'} justifyContent={"space-evenly"}>
                {state.quiz.questions.length > 0 && <Pagination
                    count={state.quiz.questions.length}
                    page={page}
                    onChange={(e, v) => setPage(v)}
                    sx={{mx: 0}}
                />}
            </Stack>
            <Button 
                color="success"
            >Post quiz</Button>
        </Stack>)

        function ValideateInput(){
            let isCorrect = !!state.quiz && state.quiz.name.length > 2 &&
                state.quiz.description.length > 10 && state.quiz.count > 0
                && !!state.quiz.questions
            if (!isCorrect)
                return 'Check quiz settings. Name longer than 2 symb., description - than 10. Count must be greater than one and less than inputed quantity of questions. Questions amount must be more than 0.'
            
            
        }
    }
}