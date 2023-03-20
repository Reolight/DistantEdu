import React, { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
import TextField from '@mui/material/TextField';
import { Stack } from "@mui/system";
import { Button } from "@mui/material";
import authService from '../api-authorization/AuthorizeService';

// props is edited subject or undefined + name of teacher

export default function SubjectNew(props) {
    const [state, setState] = useState({ subject: undefined, isReady: false, isNew: false })

    const regex = new RegExp("[\W\S]]")

    useEffect(() => {
        if (!state.isReady && props.subject === undefined) {
            setState({
                subject: {
                    name: '',
                    description: ''
                },
                isReady: true, isNew: true
            })
            return
        }

        setState({ subject: props.subject, isReady: true, isNew: false })
    }, [props])

    async function FetchSubject(e) {
        e.preventDefault();
        const token = await authService.getAccessToken();
        const auth = !token ? {} : { 'Authorization': `Bearer ${token}` }
        console.log({ 'Content-type': 'application/json', ...auth })
        const address = state.isNew ? 'subject' : `subject/${state.subject.id}`
        const response = await fetch(address, {
            headers: { 'Content-type': 'application/json', ...auth },
            method: state.isNew ? "POST" : "PUT",
            body: JSON.stringify(state.subject)
        });

        console.log(response.json())
        if (response.ok) alert('Saved');
        props.onDone();
    }

    if (state.subject === undefined) return <i>Loading...</i>
    return (<div>
        <h2>Subject creation</h2>
        <form onSubmit={FetchSubject} >
            <Stack spacing={2} direction="column" sx={{ marginBottom: 4, maxWidth: 350 }}>
                <TextField
                    required
                    error={validateName()}
                    type="text"
                    variant="outlined"
                    color="primary"
                    label="Name"
                    onChange={e => {
                        setState((prev) => (
                            {
                                ...prev,
                                subject: {
                                    ...prev.subject,
                                    name: e.target.value
                                }
                            }
                        ))}
                    }
                    value={state.subject.name}
                />
                <TextField
                    required
                    multiline
                    error={validateDesc()}
                    type='text'
                    variant="outlined"
                    color="primary"
                    label="Description"
                    onChange={e => {
                        setState((prev) => (
                            {
                                ...prev,
                                subject: {
                                    ...prev.subject,
                                    description: e.target.value
                                }
                            }
                        ))
                    }
                    }
                    value={state.subject.description}
                />
                <Button variant="outlined" color="success" type="submit">{state.isNew? `Add` : `Save`}</Button>
            </Stack>
        </form>
    </div>)

    function validateName() {
        const subjName = state.subject.name;
        return subjName.length <= 3 || subjName.length > 24 || regex.test(subjName)
    }

    function validateDesc() {
        const description = state.subject.description
        return description.length <= 40
    }
}