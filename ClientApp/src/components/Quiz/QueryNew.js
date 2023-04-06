import { Button, Checkbox, FormControlLabel, Stack, TextField } from "@mui/material";
import React, { useEffect, useState } from "react";

// props = { page, onDone }
export default function QueryNew(props){
    const [content, setContent] = useState(!!props.query? props.query.content : '')
    const [count, setCount] = useState(!!props.query? props.query.count : 1)
    const [replies, setReplies] = useState(!!props.query && !!props.query.replies? [...props.query.replies, {isCorrect: false, content: ''}]
                                                        : [{isCorrect: false, content: ''}])
    const [fastFill, setFastFill] = useState(false)
    const [fastFillText, setFFText] = useState('')
    const [isPosted, setIsPosted] = useState(false)

    useEffect(() => console.log(props), [props])
    useEffect(() => console.log(replies),[replies])

    if (!!!replies) return <p><i>Loading...</i></p>
    return(<Stack direction={'column'} spacing={2}>
        <h5>Question #{props.page+1}.</h5>
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
                if (isPosted)
                    setIsPosted(false)
                if (e.target.value > 0)
                    setCount(e.target.value)
            }}
        />

        <Stack direction={'column'} spacing={2} >
            <FormControlLabel
                label="Enable fast fill"
                control={<Checkbox
                    checked={fastFill}                
                    color="info"
                    onChange={() => setFastFill(!fastFill)}
                />}
            />
            
            {fastFill && <Stack direction={'row'} >
                <TextField 
                    fullWidth
                    multiline placeholder={`Start the line with + to enter the correct answer, with - for an incorrect answer. A line without these symbols will be considered a question.`}
                    value={fastFillText}
                    onChange={(e) => setFFText(e.target.value)}
                />
                <Stack direction={'column'} justifyContent={'flex-start'}>
                    <Button
                        color="primary"
                        disabled={fastFillText.trim().length === 0}
                        onClick={() => getFromString(fastFillText, false)}
                    >Create new</Button>
                    <Button
                        color="secondary"
                        disabled={fastFillText.trim().length === 0}
                        onClick={() => getFromString(fastFillText, true)}
                    >Add</Button>
                </Stack>
            </Stack>}

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

    function getFromString(text, add = false){
        const lines = text.split('\n');
        const obj = {
            content: '',
            replies: []
        };

        for (let i = 0; i < lines.length; i++) {
            const line = lines[i].trim();
            switch (true) {
                case line.startsWith('+'):
                    obj.replies.push({
                        isCorrect: true,
                        content: line.substring(1).trim()
                    });
                    break;
                case line.startsWith('-'):
                    obj.replies.push({
                        isCorrect: false,
                        content: line.substring(1).trim()
                    });
                    break;
                default:
                    obj.content = line.trim()
                    break;
              }
        }

        setReplies(add? [...replies.slice(0, replies.length - 1), obj.replies, {isCorrect: false, content: ''}] 
                      : [...obj.replies, {isCorrect: false, content: ''}])
        if (!!obj.content && obj.content.trim().length > 0 && !add)
            setContent(obj.content.trim())
    }
}