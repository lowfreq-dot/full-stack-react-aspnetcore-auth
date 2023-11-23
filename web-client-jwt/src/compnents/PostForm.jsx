import React, {useEffect, useRef, useState} from 'react';
import PostService from "../Services/PostService.js";
import {Button, Container, Divider, Stack, TextField} from "@mui/material";
import {ThemeProvider} from "@mui/material/styles";
import MUIRichTextEditor from "mui-rte";

import { createTheme } from '@mui/material/styles'

const myTheme = createTheme({
    // Set up your custom MUI theme here
})

const PostForm = (props) => {
    const [title, setTitle] = useState('');
    //const [body, setBody] = useState('');

    var body = '';

    const ref = useRef(null)

    useEffect(() => {
        // Этот код будет выполнен после обновления состояния
        console.log("body", body);
    }, [body]); // useEffect выполнится только при изменении состояния 'body'

    const handleSave    = (data) => {
        body = data;
        console.log("data", data);
    }

    return (
        <Stack spacing={1} my={2}>

            <TextField onChange={e => setTitle(e.target.value)} value={title} label='Заголовок'/>

            <Container sx={{"minHeight": "200px"}}>
            <ThemeProvider theme={myTheme}>
                <MUIRichTextEditor onSave={handleSave} ref={ref} inlineToolbar={true} label="Start typing..." controls={["bold", "italic", "underline", "quote", "clear"]} />
            </ThemeProvider></Container>

            {/*<div/>*/}
            {/*<TextField multiline minRows={4} onChange={e => setBody(e.target.value)} value={body} label='Содержимое поста'/>*/}
            {/*<div/>*/}
            <Divider/>
            <Button variant="outlined" onClick={async () => { ref.current?.save(); await PostService.createPost(title, body); props.updatePosts()}}>Создать пост</Button>
        </Stack>
    );
};

export default PostForm;