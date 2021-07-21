import {Card, CardActionArea, Grid, makeStyles} from "@material-ui/core";
import AlbumCard from "./AlbumCard";
import useAlbums from "../hooks/useAlbums";
import {Link} from "react-router-dom";

const useStyles = makeStyles((theme) => ({
  addAlbumContent: {
    fontSize: theme.spacing(4),
    textAlign: "center",
    padding: "64px 0",
  },
}));

const AlbumOverview = () => {
  const albums = useAlbums();
  const classes = useStyles();

  return (
      <Grid container spacing={2}>
          {albums.map(album => (
              <Grid xs={3} item key={album.id}>
                  <AlbumCard
                    id={album.id}
                    title={album.title}
                    artist={album.artist}
                    imageUrl={album.imageUrl}
                  />
              </Grid>
          ))}
        <Grid item xs={3}>
          <Card>
            <CardActionArea
              className={classes.addAlbumContent}
              component={Link}
              to="/new"
            >
              Add
            </CardActionArea>
          </Card>
        </Grid>
      </Grid>
  );
}
export default AlbumOverview;