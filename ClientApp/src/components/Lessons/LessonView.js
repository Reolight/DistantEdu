import { style } from "@mui/system";
import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Get, Patch } from "../Common/fetcher";
import ListItem from "../Common/ListItem";
import { Button, Stack, TextField } from "@mui/material";
import { TEACHER_ROLE, authenticate } from "../../roles";
import authService from "../api-authorization/AuthorizeService";

// params: [subjectId]-[order]-[count]

export default function LessonView(props){
    const { params } = useParams()
    const navigate = useNavigate()

    const [state, setState] = useState({lesson: undefined, isLoading: true, p: []})
    const [contentEdit, setContentEdit] = useState(false)
    const [user, setUser] = useState(0)

    useEffect(() => {
        async function loadUser() {
            const u = await authService.getUser()
            setUser(u)
        }

        if (user === 0)
            loadUser()
        console.log(user)
    })

    useEffect(() => {
        async function loadLesson(){
            const par = params.split('-').map(val => parseInt(val))
            const data = await Get(`lesson?subjectId=${par[0]}&order=${par[1]}`)
            setState({ lesson: data, isLoading: false, p: par });
        }

        if (params && state.isLoading)
            loadLesson()
    }, [params])

    const next = () => {
        navigate(`/lesson/${state.p[0]}-${state.p[1]+1}-${state.p[2]}`)
        setState({lesson: undefined, isLoading: true, p: []})
    }

    const prev = () => {
        navigate(`/lesson/${state.p[0]}-${state.p[1]-1}-${state.p[2]}`)
        setState({lesson: undefined, isLoading: true, p: []})
    }

    if (state.isLoading || !state.lesson)
        return <p><i>Loading....</i></p>
    return (<>
    <div>
        <h2>{state.lesson.order + 1}. {state.lesson.name}</h2>
        <p><i>{state.lesson.condition}</i></p>
        <>
            {authenticate(user.role, TEACHER_ROLE) && <>
            {!contentEdit?
                <Stack direction={'column'} >
                    {state.lesson.content}
                    <Button color='primary' onClick={ () => setContentEdit(true)
                        }>Edit</Button>
                </Stack>
                :
                <Stack direction={"column"} >
                    <TextField
                        multiline fullWidth
                        value={state.lesson.content}
                        type="text"
                        variant="outlined"
                        color="primary"
                        label="Content"
                        onChange={(e) => {
                            setState({
                                ...state,
                                lesson: {
                                    ...state.lesson,
                                    content: e.target.value
                                }
                            })
                        }}
                    />
                    <Button color='primary' onClick={ () => {
                        Patch(`lesson?lessonId=${state.lesson.lessonId}`, 
                        { content: state.lesson.content },
                        () => setContentEdit(false))
                    }}>Save</Button>
                </Stack>
            }</>}
        </>

        <br/>
        
        {state.lesson.quizzes.map((quiz) => {
            <ListItem
                item={{
                    id: quiz.id, 
                    name: quiz.name,
                    description: ((quiz) => <>
                        <p>{quiz.description}</p>
                        <p><i>Duration: {quiz.duration}</i></p>
                        <p>Count: {quiz.count}</p>

                        {quiz.startTime && <p>Started: {quiz.startTime}
                            {quiz.endTime && <> - finished {quiz.endTime}</>}
                        </p>}
                        {quiz.score && <p>{quiz.score}</p>}
                    </>)}
                }

                style={
                    quiz.score > 0? 
                        {backgroundColor: "#cefad0"}:
                        quiz.startTime !== undefined?
                            {backgroundColor: '#ffff9f'}:
                            {backgroundColor: "#f6f6f6"}
                }                
            />
        })}
        {authenticate(user.role, TEACHER_ROLE) && <Button color='primary'>Add</Button>}

        <Stack direction={'row'} justifyContent={'space-between'} >
            <Button color='primary' disabled={state.p[1] === 0} onClick={ prev }>
                Previous
            </Button>
            <Button color='primary' disabled={state.p[2] === state.p[1] + 1} onClick={ next }>
                Next
            </Button>
        </Stack>
    </div>
    </>)
}