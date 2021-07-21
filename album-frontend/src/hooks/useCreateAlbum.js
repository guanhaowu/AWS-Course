import React from 'react';

const endpoint = `${process.env.REACT_APP_API_BASE}/album`;

const useCreateAlbum = () => {
  const createAlbum = React.useCallback((newAlbum) => {
    const doCreate = async () => {
      const request = fetch(endpoint, {
        method: 'POST',
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(newAlbum)
      });

      return request.then((response) => {
        if(!response.ok) {
          console.error(response.statusText);
          return;
        }

        return response.json();
      });
    }

    return doCreate();
  }, []);

  return createAlbum;
}

export default useCreateAlbum;
