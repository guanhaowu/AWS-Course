import React from "react";
import useCreateAlbum from "../hooks/useCreateAlbum";
import AlbumForm from "./AlbumForm";
import { useHistory } from "react-router-dom";

const CreateAlbum = () => {
  const history = useHistory();
  const createAlbum = useCreateAlbum();

  const handleSubmit = React.useCallback(
    (newAlbum) => {
      createAlbum(newAlbum).then(() => {
        history.push("/");
      });
    },
    [createAlbum, history]
  );

  return <AlbumForm onSubmit={handleSubmit} submitLabel="Add" />;
};

export default CreateAlbum;
