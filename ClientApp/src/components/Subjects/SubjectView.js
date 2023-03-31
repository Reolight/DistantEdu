import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { ADMIN_ROLE, authenticate, STUDENT_ROLE, TEACHER_ROLE } from '../../roles';
import authService from '../api-authorization/AuthorizeService';
import { Button } from "@mui/material";
import ListItem from '../Common/ListItem';
import { Get } from '../Common/fetcher';

export default function SubjectView() {
    const { id } = useParams();
    const [ state, setState ] = useState({ subject: undefined, isLoading: true })
    const navigate = useNavigate();

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
            const data = await Get(`subject/${id}`)
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
            
            {!pageState.editMode && (<>
            <div>
                {state.subject.lessons.map(lesson => {
                        return <ListItem
                            key={lesson.lessonId}
                            item={{
                                id: lesson.lessonId,
                                name: lesson.name,
                                description: lesson.description
                            }}
                            role={user.role}
                            editRole={TEACHER_ROLE}
                            removeRole={TEACHER_ROLE}
                            enterQuery={(id) => {
                                const enteringdLesson = state.subject.lessons.find(l => l.lessonId === id);
                                console.log(`enter Id: ${id}, entering lesson found: `, enteringdLesson)
                                if (!!!enteringdLesson) {
                                    console.log(!!!enteringdLesson)
                                    return
                                }

                                navigate(`/lesson/${enteringdLesson.subjectId}-${enteringdLesson.order}-${state.subject.lessons.length}`)
                            }}
                            
                            editQuery={(id) => console.log(`I won't edit ${id}. Get lost`)}
                            removeQuery={(id) => console.log(`remove was pressed but nothing happened)))`)}
                        />
                    })}
            </div>
            {authenticate(user.role, TEACHER_ROLE) && (<div>
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
            </div>)}
            </>)}
        </div>
    )
}