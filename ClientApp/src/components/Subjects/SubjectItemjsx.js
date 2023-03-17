import React from 'react'
import { Button, Typography, Card, CardActionArea, CardContent, CardActions } from "@mui/material";

// props = {subject, userRole}

export default function SubjectItem(props) {

    return (<Card key={props.subject.id}>
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
                <Button size="small" color="secondary">
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