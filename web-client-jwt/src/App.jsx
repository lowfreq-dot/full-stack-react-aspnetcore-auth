/* eslint-disable no-unused-vars */
import React, { useContext, useEffect, useState } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link,
  useFetchers,
} from "react-router-dom";
import PostList from "./compnents/PostList.jsx";
import LoginForm from "./compnents/LoginForm.jsx";
import { Context } from "./main.jsx";
import { observer } from "mobx-react-lite";
import UserService from "./Services/UserService.js";
import PostService from "./Services/PostService.js";
import PostForm from "./compnents/PostForm.jsx";
import {
  AppBar,
  Button,
  Card,
  CardContent,
  CardHeader,
  Container,
  Divider,
  Paper,
  Stack,
  Toolbar,
} from "@mui/material";
import Copyright from "./compnents/Copyright.jsx";
import Typography from "@mui/material/Typography";
import MUIRichTextEditor from "mui-rte";
import {createTheme, ThemeProvider} from "@mui/material/styles";


const myTheme = createTheme({
  // Set up your custom MUI theme here
})
const App = () => {
  const { store } = useContext(Context);
  const [users, setUsers] = useState([]);
  const [posts, setPosts] = useState([]);

  useEffect(() => {
    if (localStorage.getItem("token")) {
      store.checkAuth();
    }
    getPosts();
  }, []);

  async function getUsers() {
    try {
      const response = await UserService.fetchUsers();
      setUsers(response.data);
    } catch (e) {
      console.log(e);
      alert("Не авторизован!");
    }
  }

  async function getPosts() {
    console.log("AYE");
    try {
      const response = await PostService.fetchPosts();
      setPosts(response.data);
      console.log(response.data);
    } catch (e) {
      console.log(e);
      alert("Не удалось получить список постов!");
    }
  }

  if (store.isLoading) {
    return <div>Загрузка...</div>;
  }

  return (
    <>
      <AppBar>
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            JWT
          </Typography>
          <Stack direction="row" spacing={2}>
            {store.isAuth ? (
              <>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                  {store.user}
                </Typography>
                <Button
                  variant="outlined"
                  color="inherit"
                  onClick={() => store.logout()}
                >
                  Выйти
                </Button>
              </>
            ) : (
              <LoginForm />
            )}
          </Stack>
        </Toolbar>
      </AppBar>

      <Toolbar />

      <Container maxWidth="md">
        {store.isAuth && <PostForm updatePosts={getPosts} />}

        <Stack spacing={2} my={2}>
          {posts.map((post) => (
            <Card key={post.postId}>
              <CardHeader title={post.title} />
              <Divider />
              <CardContent>
                <ThemeProvider theme={myTheme}>
                  <MUIRichTextEditor readOnly={true} defaultValue={post.body} toolbar={false}/>
                </ThemeProvider>
              </CardContent>
            </Card>
          ))}

          <Copyright />
        </Stack>
      </Container>
    </>
  );
};

export default observer(App);
