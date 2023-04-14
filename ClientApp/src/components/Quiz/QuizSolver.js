import React, { useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { Button, Pagination, Stack } from '@mui/material'
import QuestionSolver from "./QuestionSolver";
import Backend from "../Common/Backend";

// params = 
export default function QuizSolver(){
    const { params } = useParams()
    const navigate = useNavigate()

    const [answers, setAnswers] = useState()
    const [page, setPage] = useState(0)
    const [timeLeft, setTimeLeft] = useState()
    const [quiz, setQuiz] = useState(undefined);

    function CalculateTimeLeft(){ 
        const elapsedSec = Math.floor(new Date().getTime() - quiz.startTime.getTime()) / 1000;
        const leftSec = quiz.duration * 60 - elapsedSec;
        console.log(`elapsed: `, elapsedSec, ` left: `, leftSec)

        setTimeLeft(
        {
            hours: Math.floor(leftSec  / 60 / 60),
            mins: pad(Math.floor(leftSec / 60 % 60), 2),
            secs: pad(Math.floor(leftSec % 60), 2)
        })

        if (leftSec < 0){
            alert(`quiz is expired`);
            post();
            return;
        }
    }

    function pad(num, size) {
        num = num.toString();
        while (num.length < size) num = "0" + num;
        return num;
    }

    useEffect(()=>{
        async function load(quizId, lessonScoreId){
            let data = await Backend.GetInstance().Get(`answer?quizId=${quizId}&lessonScoreId=${lessonScoreId}`)
            if (data === undefined)
                alert(`Error happened`)
            
                // cuz it not date obj
            data = {...data, startTime: new Date(data.startTime) } 

            setQuiz(data)
        }

        if (quiz !== undefined)
            return
        const p = params.split('-',2)
        load(p[0], p[1])
    }, [params])

    useEffect(() => {
        if (!!!quiz || quiz.duration === 0) return
        const interval = setInterval(() => {
            CalculateTimeLeft(quiz.startTime, quiz.duration)
        }, 1000)
        return () => clearInterval(interval)
    }, [])

    useEffect(()=>{
        if (!!quiz && page < 1)
            setPage(1)
        console.log(quiz)
    }, [quiz])

    if (quiz === undefined) return <p>loading...</p>
    
    return(<>
    <Stack direction={'column'} spacing={2}>
        {timeLeft && <h3>{timeLeft.hours}:{timeLeft.mins}:{timeLeft.secs}</h3>}

        {page > 0 && <QuestionSolver 
            question={quiz.questions[page - 1]}
            onChanged={(id) => {
                setQuiz((prev) => {
                    const arr = [...prev.questions[page-1].replies];
                    arr[id].isSelected = !arr[id].isSelected
                    console.log(`asdfasdfafd`)
                    prev.questions[page-1].replies = arr;
                    return {...prev}
                })
            }}
        />}

        <Button
            color="info"
            onClick={() => post(true)}
        >Finish quiz</Button>
        
        <Pagination page={page} count={quiz.questions.length} onChange={(e, v) => setPage(v)}/>
    </Stack>
    </>)

    async function post(isUser = false){
        let isHeOrSheSure = !isUser;
        if (isUser)
            isHeOrSheSure = window.confirm(`You are going to finish test.\nAre you sure?`)
        if (!isHeOrSheSure) return;
        const answers = []
        quiz.questions.forEach(question => {
            answers.push({queryScoreId: question.queryScoreId, 
                selectedReplies: question.replies
                    .map((n,i) => {return {id: i, sel: n.isSelected}})
                    .filter(a => a.sel)
                    .map(a => a.id)
            })
        });

        const repl = await Backend.GetInstance().Post(`answer?quizScoreId=${quiz.quizScoreId}`, answers, 
            () => {
                navigate(-1)
            })

        alert(repl.ok? `Quiz accepted` : `Something wrong: ${repl.status}`)
    }
}