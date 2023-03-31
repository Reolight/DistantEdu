import { style } from "@mui/system";
import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Get } from "../Common/fetcher";
import ListItem from "../Common/ListItem";

// props = lesson.id, user

export default function LessonView(props){
    const { id } = useParams()
    const [state, setState] = useState({lesson: undefined, isLoading: true})

    useEffect(() => {
        async function loadLesson(){
            const data = await Get(`lesson/${id}`)
            setState({ lesson: data, isLoading: false });
        }

        if (id && state.isLoading)
            loadLesson()
    }, [state, id])

    if (state.isLoading || !state.lesson)
        return <p><i>Loading....</i></p>
    return (<>
    <div>
        <h2>{state.lesson.order}. {state.lesson.name}</h2>
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
    </div>
    </>)
}