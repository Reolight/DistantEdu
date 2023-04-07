import React, { useEffect, useState } from 'react'
import { Button, Typography, Card, CardActionArea, CardContent, CardActions, CardHeader } from "@mui/material";
import { authenticate } from '../../roles';

// props = {item.(id,name,description), editRole, removeRole, 
//          userRole, enterQuery, editQuery, removeQuery, style }

export default function ListItem(props) {
    // useEffect(() => console.log("List item received props: ", props),[props])

    return (<Card key={props.item.id} >
        <CardActionArea>
            <CardContent style={props.style} >
                <Typography gutterBottom variant="h5" component="div">
                    {typeof props.item.name === 'function'?
                        props.item.name() : props.item.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {typeof props.item.description === "function"?
                        props.item.description() : props.item.description}
                </Typography>
            </CardContent>
        </CardActionArea>
        <CardActions>
            {props.enterQuery && <Button size="small" color="primary" onClick={() => {
                console.log(`[${props.item.id}] ${props.item.name}: pressed enter button`)
                props.enterQuery(props.item.id)
            }}>
                Enter
            </Button>}

            {props.role && props.editRole && authenticate(props.role, props.editRole) && (
                <Button size="small" color="secondary" onClick={() => {
                    console.log(`[${props.item.id}] ${props.item.name}: pressed edit button`)
                    props.editQuery(props.item.id)
                }} >
                    Edit
                </Button>
            )}
            
            {props.role && props.removeRole && authenticate(props.role, props.removeRole) && (
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