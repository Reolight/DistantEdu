import { style } from "@mui/system";
import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Get } from "../Common/fetcher";
import ListItem from "../Common/ListItem";
import { Button, Stack } from "@mui/material";

// params: [subjectId]-[order]-[count]

export default function LessonView(props){
    const { params } = useParams()
    const [state, setState] = useState({lesson: undefined, isLoading: true, p: []})
    const navigate = useNavigate()

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
            {state.lesson.content}
        </>
        <br/>
        {state.lesson.quizzes.map((quiz, i, quizzes) => {
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