import { Button, Checkbox, Stack, TextField } from "@mui/material";
import React, { useEffect, useState } from "react";

// props = { page, onDone }
export default function QueryNew(props){
    const [content, setContent] = useState(!!props.query? props.query.content : '')
    const [count, setCount] = useState(!!props.query? props.query.count : 0)
    const [replies, setReplies] = useState(!!props.query? [...props.query.replies, {isCorrect: false, content: ''}]
                                                        : [{isCorrect: false, content: ''}])
    const [isPosted, setIsPosted] = useState(false)

    useEffect(() => console.log(props), [props])
    useEffect(() => console.log(replies),[replies])

    if (!!!replies) return <p><i>Loading...</i></p>
    return(<Stack direction={'column'} spacing={2}>
        <h5>Question #{props.page}.</h5>
        <TextField 
            multiline
            label='Question content'
            value={content}
            onChange={(e) => { setContent(e.target.value); setIsPosted(false)}}
        />
        <TextField 
            label="Answer's count" 
            type='number' 
            sx={{width: 150}}
            value={count}
            onChange={(e)=>{
                setReplies((prev) => {
                    const toDel = [];
                    prev.map((prevRepl, i) => {
                        // mark to delete empty entry (except last one)
                        if (prevRepl === undefined ||
                            (prevRepl.isCorrect === undefined && 
                            prevRepl.content === undefined &&
                            i !== prev.length - 1))
                            
                            toDel = [...toDel, i];
                    })
                        // deleting empty in reverse order
                    for (let j = toDel.length - 1; j >= 0; j--) {
                        prev.splice(toDel[j], 1)
                    }

                    return prev
                })

                setCount(e.target.value)
            }}
        />

        <Stack direction={'column'} spacing={2} >
            {replies.map((reply, i) => 
                <Stack key={i} direction={'row'} spacing={1}>
                    <Checkbox 
                        checked={reply.isCorrect}
                        color="success" 
                        onChange={(e) => {
                            setIsPosted(false)
                            setReplies((prev) => {
                                prev[i] = {...prev[i], isCorrect: !reply.isCorrect }
                                return [...prev]
                            })
                        }}
                    />
                    <TextField 
                        label={`Reply ${i+1}`} 
                        fullWidth
                        multiline placeholder="Enter variant of an answer"
                        value={reply.content}
                        onChange={(e) => {
                            let t = replies
                            t[i].content = e.target.value
                            if (i + 1 === replies.length && replies[i].name !== ""){
                                t.push({isCorrect: false, content: ""})
                            } else if (i + 2 === replies.length && replies[i].content === ""
                                && replies[i+1].content === "")
                            {
                                t.pop()
                            }
                            setIsPosted(false)
                            setReplies([...t])
                        }}
                    />
                </Stack> 
            )}
        </Stack>

        <Button 
            label='Save' 
            disabled={!Validate() || isPosted}
            onClick={ () => {
                props.onDone({
                    content: content, 
                    count: count,
                    replies: replies.slice(0, replies.length-1)
                })

            setIsPosted(true)
        }} 
        >Save</Button>
    </Stack>)

    function Validate(){
        if (replies.length <= count)
            return false;
        let isReady = true;
        for (let i = 0; i < replies.length - 1; i++)
            isReady &= !!replies[i].content

        console.log(isReady)
        return isReady;
    }
}