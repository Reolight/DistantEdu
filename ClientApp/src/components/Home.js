import React, { Component, useEffect, useState } from 'react';
import authService from './api-authorization/AuthorizeService';
import { Button, Typography, Card, CardActionArea, CardContent, CardActions } from "@mui/material";

const displayName = Home.name;

export function Home() {

    const [state, setState] = useState({ Subjects: [], isLoading: true})

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
                    return (<Card>
                        <CardActionArea>
                            <CardContent>
                                <Typography gutterBottom variant="h5" component="div">
                                    {subject.name}
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    {subject.description}
                                </Typography>
                            </CardContent>
                        </CardActionArea>
                    </Card>)
                })}</>
            )}
      </div>
    );
}
