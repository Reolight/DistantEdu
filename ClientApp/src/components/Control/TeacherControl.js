import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom'
import Backend from '../Common/Backend';
import authService from '../api-authorization/AuthorizeService';
import { TEACHER_ROLE, authenticate } from '../../roles';
import ListItem from '../Common/ListItem';

// params = quizId.
export default function TeacherControl() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [user, setUser] = useState(0)
    const [state, setState] = useState({quizInfo: undefined, isReady: false})

    useEffect(() => {
        async function load(){
            const data = await Backend.GetInstance().Get(`teacher?quizId=${id}`)
            const user = await authService.getUser()
            setUser(user)
            if (!!!user || !authenticate(user.role, TEACHER_ROLE))
                navigate(-1);
            setState({
                quizInfo: data,
                isReady: true
            });
        }

        if (!!!state && !state.isReady || !!!user)
            load()
    }, [id])

    if (!!!state && !state.isReady)
        return <p><i>Loading...</i></p>

    return(<>
        {state.quizInfo.map((quiz) => {
            return (<ListItem
                key={`li${quiz.quizId}`}
                item={{
                    id: quiz.quizId, 
                    name: quiz.solvedBy,
                    description: (() => <>
                        {quiz.startTime && <p><b>Started:</b> {new Date(quiz.startTime).toLocaleString()}
                            {quiz.endTime && <> - <b>finished</b> {new Date(quiz.endTime).toLocaleString()}</>}
                        </p>}
                        {!!quiz.endTime && <p><b>Score</b>:{quiz.score}</p>}
                    </>)}
                }

                style={
                    quiz.score > 0? 
                        {backgroundColor: "#cefad0"}:
                        quiz.startTime === undefined?
                            {backgroundColor: '#ffff9f'}:
                            {backgroundColor: "#f6f6f6"}
                }                
            />
        )})}
    </>)
}