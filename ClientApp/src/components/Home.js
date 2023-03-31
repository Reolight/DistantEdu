import React, { useEffect, useState } from 'react';
import { Button, Stack } from "@mui/material";
import authService from './api-authorization/AuthorizeService';
import SubjectNew from './Subjects/SubjectNew';
import {  Route, Routes, useNavigate } from 'react-router-dom';
import ListItem from './Common/ListItem';
import { adminRole, ADMIN_ROLE, authenticate, studentRole, teacherRole, TEACHER_ROLE } from '../roles';
import SubjectView from './Subjects/SubjectView';
import { Get } from './Common/fetcher';

export function Home() {
    const [pageState, setPageState] = useState({ showList: true, editMode: false, editableSub: undefined })
    const navigate = useNavigate()

    const [user, setUser] = useState(0);
    const [state, setState] = useState({ Subjects: [], isLoading: true})

    useEffect(() => {
        async function loadUser() {
            const u = await authService.getUser()
            setUser(u)
        }

        if (user === 0)
            loadUser()
    })

    useEffect(() => {
        async function loadSubjInfo() {
            const data = await Get('subject')
            setState({ Subjects: data, isLoading: false });
        }
        if (state.isLoading) loadSubjInfo()
        else console.log(state, user)
    }, [state])

    function editChild(id) {
        var subjToEdit = state.Subjects.find(sub => sub.id === id)
        if (subjToEdit !== undefined)
            setPageState({ showList: false, editMode: true, editableSub: subjToEdit })
    }

    if (state.isLoading && (!!!user || !!!state.Subjects)) return <i>Loading</i>

    return (
        <div>
            <Stack spacing={2} direction="column">
                {!state.isLoading && pageState.showList && (<>
                    {state.Subjects.map(subject => {
                        return <ListItem
                            key={subject.id}
                            style={subject.subscriptionId > 0? {backgroundColor: "#cefad0"} : {backgroundColor: "#f6f6f6"}}
                            item={subject}
                            role={user.role}
                            editRole={TEACHER_ROLE}
                            removeRole={ADMIN_ROLE}
                            enterQuery={(id) => {
                                const enteringdSub = state.Subjects.find(s => s.id === id);
                                if ((!!!enteringdSub) || (!enteringdSub.subscriptionid && !window.confirm(`You are not subscribed. Subscribe and enter?`))) {
                                    console.log(!!!enteringdSub)
                                    return
                                }

                                navigate(`/subject/${enteringdSub.id}`)
                            } }
                            editQuery={editChild}
                            removeQuery={(id) => console.log(`remove was pressed but nothing happened)))`)}
                        />
                    })}</>
                )}
                {!pageState.editMode && authenticate(user.role, TEACHER_ROLE) && (<p>
                    <Button
                        variant='outlined'
                        color="secondary"
                        type="submit"
                        style={{ maxWidth: '250px' }}
                        onClick={() => {
                            console.log(`add clicked.`)
                            setPageState({ showList: false, editMode: true })
                        }}
                    >Add</Button>
                </p>)}

                {pageState.editMode && <SubjectNew
                    author={user.name}
                    onDone={() => setPageState({ showList: true, editMode: false, editableSub: undefined })}
                    subject={pageState.editableSub}
                />}
            </Stack>

            <Routes>
                <Route path="/subject/:id" element={<SubjectView user={user} /> } />
            </Routes>
            
        </div>
    );
}
