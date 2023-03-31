import React, { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
import TextField from '@mui/material/TextField';
import { Stack } from "@mui/system";
import { Button, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import authService from '../api-authorization/AuthorizeService';
import { Post, Put } from '../Common/fetcher';

// props = subjectId, order, onDone. It may be `lesson` itself

export default function LessonNew(props) {
    const [state, setState] = useState({ lesson: undefined, isReady: false, isNew: false })
    const regex = new RegExp("[\W\S]]")

    useEffect(() => {
        if (!state.isReady && props.lesson === undefined) {
            setState({
                lesson: {
                    name: '',
                    subjectId: props.subjectId,
                    description: '',
                    order: props.order,
                    content: '',
                    condition: 0,
                    tests: []
                },
                isReady: true, isNew: true
            })
            return
        }

        setState({ lesson: props.lesson, isReady: true, isNew: false })
        console.log(state)
    }, [props])

    async function FetchSubject(e) {
        e.preventDefault();
        state.isNew? await Post(`lesson?subjectId=${props.subjectId}`, state.lesson, props.onDone)
                   : await Put(`lesson`, state.lesson, props.onDone)
    }

    if (!state.isReady) return <i>Loading...</i>

    return (<div>
        {state.isNew? <h2>Lesson creation</h2> : <h2>Lesson edit</h2>}
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
                                lesson: {
                                    ...prev.lesson,
                                    name: e.target.value
                                }
                            }
                        ))}
                    }
                    value={state.lesson.name}
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
                                lesson: {
                                    ...prev.lesson,
                                    description: e.target.value
                                }
                            }
                        ))
                    }
                    }
                    value={state.lesson.description}
                />
                <FormControl>
                    <InputLabel id="select-condition">Select condition to pass lesson</InputLabel>
                    <Select
                        labelId="select-condition"
                        value={state.lesson.condition}
                        label="Condition"
                        onChange={e => {
                            setState((prev) => (
                                {
                                    ...prev,
                                    lesson: {
                                        ...prev.lesson,
                                        condition: e.target.value
                                    }
                                }
                            ))
                        }}
                    >
                        <MenuItem value={0}>Read only</MenuItem>
                        <MenuItem value={1}>Single test</MenuItem>
                        <MenuItem value={2}>Key tests only</MenuItem>
                        <MenuItem value={3}>All tests</MenuItem>
                    </Select>
                </FormControl>

                <TextField
                    required
                    multiline
                    error={state.lesson.content.length < 100}
                    type='text'
                    variant="outlined"
                    color="primary"
                    label="Content"
                    placeholder='100 symbols at least'
                    onChange={e => {
                        setState((prev) => (
                            {
                                ...prev,
                                lesson: {
                                    ...prev.lesson,
                                    content: e.target.value
                                }
                            }
                        ))
                    }
                    }
                    value={state.lesson.content}
                />
                <Button variant="outlined" color="success" type="submit">{state.isNew? `Add` : `Save`}</Button>
            </Stack>
        </form>
    </div>)

    function validateName() {
        const subjName = state.lesson.name;
        return subjName.length <= 3 || subjName.length > 30 || regex.test(subjName)
    }

    function validateDesc() {
        const description = state.lesson.description
        return description.length <= 40 || description.length > 220
    }
}