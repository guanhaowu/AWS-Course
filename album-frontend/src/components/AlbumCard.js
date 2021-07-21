import {
    Card,
    CardActionArea,
    CardContent,
    CardMedia,
    makeStyles,
    Typography,
} from "@material-ui/core";
import { Link } from "react-router-dom";

const useStyles = makeStyles({
    media: {
        height: 240,
    },
});

const AlbumCard = ({ id, name, artist, imageUrl }) => {
    const classes = useStyles();

    return (
      <Card>
          <CardActionArea component={Link} to={`/${id}`}>
              <CardMedia
                className={classes.media}
                image={imageUrl}
                title="Album cover"
              />
              <CardContent>
                  <Typography gutterBottom variant="h5" component="h2">
                      {name}
                  </Typography>
                  <Typography variant="body2" color="textSecondary" component="p">
                      {artist}
                  </Typography>
              </CardContent>
          </CardActionArea>
      </Card>
    );
};

export default AlbumCard;
