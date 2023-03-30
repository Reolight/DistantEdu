import React, { useState } from 'react'
import { Button, Typography, Card, CardActionArea, CardContent, CardActions } from "@mui/material";
import { authenticate } from '../../roles';

// props = {item, editRole, removeRole, userRole,
//          editQuery, removeQuery, style }

export default function ListItem(props) {

    return (<Card key={props.item.id} >
        <CardActionArea>
            <CardContent style={props.style} >
                <Typography gutterBottom variant="h5" component="div">
                    {props.item.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {props.item.description}
                </Typography>
            </CardContent>
        </CardActionArea>
        <CardActions>
            <Button size="small" color="primary" onClick={() => {
                console.log(`[${props.item.id}] ${props.item.name}: pressed enter button`)
                props.enterQuery(props.item.id)
            } }>
                Enter
            </Button>
            {authenticate(props.role, props.editRole) && (
                <Button size="small" color="secondary" onClick={() => {
                    console.log(`[${props.item.id}] ${props.item.name}: pressed edit button`)
                    props.editQuery(props.item.id)
                }} >
                    Edit
                </Button>
            )}
            {authenticate(props.role, props.removeRole) && (
                <Button size='small' color="error" onClick={() => {
                    console.log(`[${props.item.id}] ${props.item.name}: pressed REMOVE button`)
                    let response = window.confirm(`You pressed remove for ${props.item.name}. Are you sure?`)
                    if (response) {
                        props.removeQuery(props.item.id)
                    }
                } }>
                    Remove
                </Button>
            )}
        </CardActions>
    </Card>)
}