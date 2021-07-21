import React from 'react';
import { useHistory, useParams } from "react-router-dom";
import useAlbum from "../hooks/useAlbum";
import AlbumForm from "./AlbumForm";

const AlbumDetail = () => {
  const history = useHistory();
  const { albumId } = useParams();
  const { album, updateAlbum, removeAlbum } = useAlbum(albumId);

  const handleSubmit = React.useCallback(
    (album) => {
      updateAlbum(album).then(() => {
        history.push("/");
      });
    },
    [updateAlbum, history]
  );

  const handleRemove = React.useCallback(() => {
    removeAlbum().then(() => {
      history.push("/");
    });
  }, [removeAlbum, history]);

  return album ? (
    <AlbumForm
      album={album}
      onSubmit={handleSubmit}
      submitLabel="Edit"
      onRemove={handleRemove}
    />
  ) : null;
};

export default AlbumDetail;