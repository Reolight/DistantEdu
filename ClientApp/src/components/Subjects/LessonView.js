import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

// props = lesson.id, user

export default function LessonView(props){
    const { id } = useParams()
    const [state, setState] = useState({lesson: undefined, isLoading: true})

    useEffect(() => {
        async function loadLesson(){
            console.log(id)
            const token = await authService.getAccessToken();
            const response = await fetch(`lesson/${id}`, {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });

            const data = await response.json();
            console.log(data)
            setState({ lesson: data, isLoading: false });
        }

        if (id && state.isLoading)
            loadLesson()
    }, [state, id])

    return (<></>)
}