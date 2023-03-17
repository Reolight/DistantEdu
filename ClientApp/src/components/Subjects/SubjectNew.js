import React, { useEffect, useNavigaror } from 'react'

// props is edited subject or undefined + name of teacher

export default function SubjectNew(props) {
    const nav = useNavigaror()
    const [state, setState] = useState({ subject: undefined, isReady: false, isNew: false })
    const [userName] = useState(props.name)

    useEffect(() => {
        if (!state.isReady && props.subject === undefined) {
            setState({ subject: undefined, isReady: true, isNew: true })
            return
        }

        setState({ subject: props.subject, isReady: true, isNew: false })
    })

    async function FetchSubject() {
        e.preventDefault();
        const token = await authService.getAccessToken();
        const auth = !token ? {} : { 'Authorization': `Bearer ${token}` }
        console.log({ 'Content-type': 'application/json', ...auth })
        const address = state.isNew ? 'subject' : `subject/${subject.id}`
        const response = await fetch(address, {
            headers: { 'Content-type': 'application/json', ...auth },
            method: state.isNew ? "POST" : "PUT",
            body: JSON.stringify(state.subject)
        });

        console.log(response.json())
        if (response.ok) alert('Saved');

    }

    return (<div>
        <form onSubmit={FetchSubject} >
            
        </form>
    </div>)
}