import React, { useState } from 'react'
import { Button, Typography, Card, CardActionArea, CardContent, CardActions } from "@mui/material";
import SubjectNew from './SubjectNew';

// props = {subject, userRole}

export default function SubjectItem(props) {

    return (<Card key = { props.subject.id } >
        <CardActionArea>
            <CardContent>
                <Typography gutterBottom variant="h5" component="div">
                    {props.subject.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {props.subject.description}
                </Typography>
            </CardContent>
        </CardActionArea>
        <CardActions>
            <Button size="small" color="primary">
                Enter
            </Button>
            {(props.role === "teacher" || props.role === 'admin') && (
                <Button size="small" color="secondary" onClick={() => {
                    console.log(`[${props.subject.id}] ${props.subject.name}: pressed edit button`)
                    props.editQuery(props.subject.id)
                }} >
                    Edit
                </Button>
            )}
            {(props.role === 'admin') && (
                <Button size='small' color="error">
                    Remove
                </Button>
            )}
        </CardActions>
    </Card>)
}