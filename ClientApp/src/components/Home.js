import React, { Component, useEffect, useState } from 'react';
import { Button, Stack } from "@mui/material";
import authService from './api-authorization/AuthorizeService';
import SubjectItem from './Subjects/SubjectItemjsx';
import SubjectNew from './Subjects/SubjectNew';

const displayName = Home.name;

export function Home() {
    const [pageState, setPageState] = useState({ showList: true, editMode: false, editableSub: undefined })

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
            const token = await authService.getAccessToken();
            const response = await fetch('subject', {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });
            const data = await response.json();
            console.log(data)
            setState({ Subjects: data, isLoading: false });
        }
        if (state.isLoading) loadSubjInfo()
        else console.log(state)
    }, [state])

    function editChild(id) {
        var subjToEdit = state.Subjects.find(sub => sub.id === id)
        if (subjToEdit !== undefined)
            setPageState({ showList: false, editMode: true, editableSub: subjToEdit })
    }

    return (
        <div>
            <Stack spacing={2} direction="column">
                {!state.isLoading && pageState.showList && (<>
                    {state.Subjects.map(subject => {
                        return <SubjectItem subject={subject} role={user.role} editQuery={editChild} key={subject.id} />
                    })}</>
                )}
                {!pageState.editMode && (user.role === "teacher" || user.role === "admin") && (<p>
                    <Button
                        variant='outlined'
                        color="secondary"
                        type="submit"
                        style={{ maxWidth: '250px' }}
                        onClick={() => {
                            console.log(`add clicked.`)
                            if ((user.role !== "teacher" && user.role !== "admin") || user.role === 'student')
                                return
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
        </div>
    );
}
