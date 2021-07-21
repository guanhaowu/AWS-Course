import React from "react";

const endpoint = `${process.env.REACT_APP_API_BASE}/album`;

const useAlbums = () => {
  const [albums, setAlbums] = React.useState([]);

  React.useEffect(() => {
    const request = fetch(endpoint);

    request
      .then((apiResponse) => {
        if (!apiResponse.ok) console.error(apiResponse.statusText)
        return apiResponse.json();
      })
      .then((apiResult) => {
        setAlbums(apiResult);
      });
  }, []);
  return albums;
};
export default useAlbums;