import { style } from "@mui/system";
import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ListItem from "../Common/ListItem";

// props = lesson.id, user

export default function LessonView(props){
    const { id } = useParams()
    const [state, setState] = useState({lesson: undefined, isLoading: true})

    useEffect(() => {
        async function loadLesson(){
            console.log(id)
            const token = await authService.getAccessToken();
            const response = await fetch(`lesson/${id}`, {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });

            const data = await response.json();
            console.log(data)
            setState({ lesson: data, isLoading: false });
        }

        if (id && state.isLoading)
            loadLesson()
    }, [state, id])

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