import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { ADMIN_ROLE, STUDENT_ROLE, TEACHER_ROLE } from '../../roles';
import authService from '../api-authorization/AuthorizeService';
import { Button } from "@mui/material";

export default function SubjectView() {
    const { id } = useParams();
    const [ state, setState ] = useState({ subject: undefined, isLoading: true })

    const [pageState, setPageState] = useState({ editMode: false, editedInstance: undefined })
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
        async function loadSubjInfo(id) {
            console.log(id)
            const token = await authService.getAccessToken();
            const response = await fetch(`subject/${id}`, {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });

            const data = await response.json();
            console.log(data)
            setState({ subject: data, isLoading: false });
        }
        if (!!state && state.isLoading && id !== undefined)
            loadSubjInfo(id)
        else console.log(state)
    }, [id])

    if (state.isLoading) return <i>Loading...</i>
    if (state.subject === undefined) return <i><strong>Subject doesn't exist</strong></i>

    return (
        <div>
            <h2>{state.subject.name}</h2>
            <p><i>Author: {state.subject.author}</i></p>

            <p>{state.subject.description}</p>
            
            {!pageState.editMode && (user.role === TEACHER_ROLE || user.role === ADMIN_ROLE) && (<>
            <p>
                {state.subject.lessons.map(lesson => {
                        return <ListItem
                            key={lesson.id}
                            item={lesson}
                            role={user.role}
                            editRole={TEACHER_ROLE}
                            removeRole={TEACHER_ROLE}
                            enterQuery={(id) => {
                                const enteringdLesson = state.subject.lessons.find(s => s.id === id);
                                if (!!!enteringdLesson) {
                                    console.log(!!!enteringdLesson)
                                    return
                                }

                                navigate(`/lesson/${enteringdLesson.id}`)
                            } }
                            editQuery={editChild}
                            removeQuery={(id) => console.log(`remove was pressed but nothing happened)))`)}
                        />
                    })}
            </p>
            <p>
                <Button
                    variant='outlined'
                    color="secondary"
                    type="submit"
                    style={{ maxWidth: '250px' }}
                    onClick={() => {
                        console.log(`add clicked.`)
                        setPageState({ editMode: true })
                    }}
                >Add</Button>
            </p></>)}
        </div>
    )
}