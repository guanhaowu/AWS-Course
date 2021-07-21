import React from "react";

const endpoint = `${process.env.REACT_APP_API_BASE}/album`;

const useAlbum = (id) => {
  const [album, setAlbum] = React.useState();

  React.useEffect(() => {
    const request = fetch(`${endpoint}/${id}`);

    request
      .then((apiResponse) => {
        if (!apiResponse.ok) {
          console.error(apiResponse.statusText);
          return;
        }

        return apiResponse.json();
      })
      .then((apiResult) => {
        setAlbum(apiResult);
      });
  }, [id]);

  const updateAlbum = React.useCallback(
    (changedAlbum) => {
      console.log("ChangedAlbum:" , changedAlbum)

      const request = fetch(`${endpoint}/${id}`, {
        method: "PUT",
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(changedAlbum),
      });

      request.then((apiResponse) => {
        if (!apiResponse.ok) {
          console.error(apiResponse.statusText);
        }
        setAlbum(changedAlbum);
      });

      return request;
    },
    [id]
  );

  const removeAlbum = React.useCallback(() => {
    const request = fetch(`${endpoint}/${id}`, {
      method: "DELETE",
    });

    request.then((apiResponse) => {
      if (!apiResponse.ok) {
        console.error(apiResponse.statusText);
      }
    });

    return request;
  }, [id]);

  return React.useMemo(
    () => ({
      album,
      updateAlbum,
      removeAlbum,
    }),
    [album, updateAlbum, removeAlbum]
  );
};

export default useAlbum;
