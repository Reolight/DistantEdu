import React, { useState } from "react";
import { Card, CardContent, CardHeader, Checkbox, FormControlLabel, Stack } from "@mui/material";

//props = question: queryId, queryScoreId, content, isReplied, [isCorrect]
//        replies (replyId, repliedId, content, isSelected)
//funcs:  onDone, onChanged
export default function QuestionSolver(props){

    if (props === undefined) return <p>question disappeared?</p>

    return (
    <Stack direction={"column"} spacing={2}>
        <Card>            
            <CardContent>
                <h3>{props.question.content}</h3>
                {props.question.replies.map((reply, id) => 
                <Stack direction={'column'} spacing={1} key={reply.replyId}>
                    <FormControlLabel
                        label={reply.content}
                        control={<Checkbox
                            checked={reply.isSelected}                
                            color="info"
                            onChange={() => props.onChanged(id)}
                    />}
                />
                </Stack>
                )}
            </CardContent>
        </Card>
    </Stack>)
}