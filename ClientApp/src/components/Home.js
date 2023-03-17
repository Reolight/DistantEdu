import React, { Component, useEffect, useState } from 'react';
import authService from './api-authorization/AuthorizeService';
import { Button, Typography, Card, CardActionArea, CardContent, CardActions } from "@mui/material";
import SubjectItem from './Subjects/SubjectItemjsx';

const displayName = Home.name;

export function Home() {
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
            setState({ Subjects: data, isLoading: false });
        }
        if (state.isLoading) loadSubjInfo()
        else console.log(state.Subjects)
    }, [state])
        
    return (
        <div>
            {!state.isLoading && (<>
                {state.Subjects.map(subject => {
                    return <SubjectItem subject={subject} role={user.role} />
                })}</>
            )}
      </div>
    );
}
