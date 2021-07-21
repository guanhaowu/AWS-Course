import './App.css';
import {AppBar, Container, makeStyles, Toolbar, Typography} from "@material-ui/core";
import {BrowserRouter, Switch, Route, Link} from "react-router-dom";
import AlbumOverview from "./components/AlbumOverview";
import AlbumDetail from "./components/AlbumDetail";
import CreateAlbum from "./components/CreateAlbum";

const useStyles = makeStyles(theme => ({
  container: {
    paddingTop: theme.spacing(4),
  },
  toolbar: theme.mixins.toolbar,
  logoLink: {
    textDecoration: "none",
    color: theme.palette.primary.contrastText,
  },

}));

function App() {
  const classes = useStyles();

  return (
    <BrowserRouter>
      <AppBar position={"sticky"}>
        <Toolbar>
          <Typography
            variant="h6"
            component={Link}
            to="/"
            className={classes.logoLink}
          >
            Album Api
          </Typography>
        </Toolbar>
      </AppBar>
      <Container className={classes.container}>
        <Switch>
          <Route path={"/"} exact>
            <AlbumOverview/>
          </Route>
          <Route path="/new">
            <CreateAlbum/>
          </Route>
          <Route path="/:albumId">
            <AlbumDetail/>
          </Route>
        </Switch>
      </Container>
    </BrowserRouter>
  );
}

export default App;
