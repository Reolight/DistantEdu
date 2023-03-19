import { Button, Stack, TextField, Checkbox } from "@mui/material";
import FormControlLabel from '@mui/material/FormControlLabel';
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import authService from "./api-authorization/AuthorizeService";

export default function Aezakmi() {
    const nav = useNavigate()
    const [name, setName] = useState('')
    const [teacherchecked, setTC] = useState(false)

    const [user, setUser] = useState()
    useEffect(() => {
        async function userInit() {
            const u = await authService.getUser()
            setUser(u)
        }

        if (user === undefined)
            userInit();
        console.log(user);
    }, [user])

    return (<>
        <form onSubmit={async (e) => {
            e.preventDefault();
            console.log(user)
            const token = await authService.getAccessToken();
            const auth = !token ? {} : { 'Authorization': `Bearer ${token}` }
            console.log({ 'Content-type': 'application/json', ...auth })
            const response = await fetch(`Toggler/aezakmi`, {
                headers: { 'Content-type': 'application/json', ...auth },
                method: "POST",
                body: JSON.stringify({ name: name, teacherChecked: user.role === `student` })
            });

            console.log(response)
        } }>
            <Stack spacing={2} direction="column" sx={{marginBottom: 4, maxWidth: 250}} >
                <TextField
                        required
                        type="text"
                        variant="outlined"
                        color="primary"
                        label="Ваше имя"
                        onChange={e => setName(e.target.value)}  
                        value={name}
                />

                <FormControlLabel
                    control={<Checkbox
                        checked={teacherchecked}
                        onChange={() => setTC(!teacherchecked)}
                    />}
                    label="Make me a teacher"
                    labelPlacement="end"
                />
            <Button variant="outlined" color="success" type="submit">Отправить</Button>
            </Stack>
        </form>
        </>)
}