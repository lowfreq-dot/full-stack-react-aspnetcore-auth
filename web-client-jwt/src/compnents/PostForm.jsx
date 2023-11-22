import {useState} from 'react';
import PostService from "../Services/PostService.js";
import {Button, Stack, TextField} from "@mui/material";

const PostForm = (props) => {
    const [title, setTitle] = useState('');
    const [body, setBody] = useState('');

    return (
        <Stack spacing={1} my={2}>
            <TextField onChange={e => setTitle(e.target.value)} value={title} label='Заголовок'/>
            <div/>
            <TextField multiline minRows={4} onChange={e => setBody(e.target.value)} value={body} label='Содержимое поста'/>
            <div/>
            <Button variant="outlined" onClick={async () => { await PostService.createPost(title, body); props.updatePosts()}}>Создать пост</Button>
        </Stack>
    );
};

export default PostForm;