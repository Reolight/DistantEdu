import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { authenticate, TEACHER_ROLE } from '../../roles';
import authService from '../api-authorization/AuthorizeService';
import { Button, LinearProgress, Paper, Stack, Typography } from "@mui/material";
import ListItem from '../Common/ListItem';
import LessonNew from '../Lessons/LessonNew';
import Backend from '../Common/Backend';

export default function SubjectView() {
    const { id } = useParams();
    const navigate = useNavigate();
    
    const [ state, setState ] = useState({ subject: undefined, isLoading: true })
    const [pageState, setPageState] = useState({ editMode: false, editable: undefined })
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
            const data = await Backend.GetInstance().Get(`subject/${id}`)
            setState({ subject: data, isLoading: false });
        }
        if (!!state && state.isLoading && id !== undefined)
            loadSubjInfo(id)
        else console.log(state)
    }, [id, state.isLoading])

    function editChild(id) {
        var lessonEditable = state.subject.lessons.find(l => l.lessonId === id)
        if (lessonEditable !== undefined)
            setPageState({ editMode: true, editable: lessonEditable })
    }

    if (state.isLoading) return <i>Loading...</i>
    if (state.subject === undefined) return <i><strong>Subject doesn't exist</strong></i>

    return (
        <div>
            {!pageState.editMode && (<>
            <Paper elevation={12} sx={{my: 4, p: 4, borderRadius: 4}}>
                <Stack direction={'column'} spacing={2} >
                    <Typography variant='h3'>{state.subject.name}</Typography>
                    <Typography><i>Author: {state.subject.author}</i></Typography>

                    <Typography>{state.subject.description}</Typography>
                    {state.subject.progress > 0? <Stack direction={'row'} spacing={2} alignItems={'center'} >
                        <Typography>Progress: {state.subject.progress}%</Typography>
                        <LinearProgress 
                            sx={{width: '50%', height: '1.5vh'}}
                            variant='determinate' 
                            value={state.subject.progress} 
                        />
                    </Stack> : <></>}
                </Stack>
            </Paper>

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
                            
                            editQuery={ editChild }
                            removeQuery={(id) => Backend.GetInstance().Delete(`lesson?lessonId=${id}`)}
                            style={ lesson.isPassed? {backgroundColor: "#cefad0"} : {}}
                        />
                    })}
            </div>

            {!pageState.editMode && authenticate(user.role, TEACHER_ROLE) && (<div>
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
            
            {pageState.editMode && <LessonNew
                onDone={() => { 
                    setPageState({ editMode: false, editable: undefined });
                    setState({ subject: undefined, isLoading: true })
                }}

                lesson={pageState.editable}
                order={state.subject.lessons.length}
                subjectId={state.subject.id}                    
            />}
        </div>
    )
}