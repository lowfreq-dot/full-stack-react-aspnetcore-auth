import {useContext, useState} from 'react';
import {Context} from "../main.jsx";
import {observer} from "mobx-react-lite";
import {Button, Paper, Stack, TextField} from "@mui/material";

const LoginForm = () => {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const {store} = useContext(Context);

    return (
        <Stack direction="row" spacing={2}>
            <TextField component={Paper} variant="filled" size="small" onChange={e => setEmail(e.target.value)} value={email} label="Email"/>
            <TextField component={Paper} variant="filled" size="small" onChange={e => setPassword(e.target.value)} value={password} type="password" label="Password"/>
            <Button color="inherit" onClick={() => store.login(email, password)}>Login</Button>
            <Button color="inherit" onClick={() => store.registration(email, password)}>Registration</Button>
        </Stack>
    );
};

export default observer(LoginForm);