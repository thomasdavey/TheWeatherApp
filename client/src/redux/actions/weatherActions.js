import axios from 'axios';

export const submitLocation = (location) => dispatch => {
  dispatch(setLoading());

  axios.get(`/Weather?location=${location}`)
    .then((res) => {
      console.log(res);

      dispatch(setWeather(res.data));
    })
    .catch((e) => {
      if (e.response.status === 404) {
        dispatch(setError('The location could not be found. Please adjust your input and try again.'));
      } else if (e.response.status === 400) {
        dispatch(setError('The location input was invalid. Please adjust your input and try again.'));
      } else {
        dispatch(setError('An unknown error ocurred finding the location. Please try again.'));
      }
    });
};

export const setLoading = () => dispatch => {
  dispatch({ type: 'WEATHER_LOADING' });
};

export const setWeather = (weather) => dispatch => {
  dispatch({
    type: 'WEATHER_LOADED',
    weather: weather
  });
};

export const setError = (message) => dispatch => {
  dispatch({
    type: 'WEATHER_ERROR',
    error: message
  });
};

export const clearErrors = () => dispatch => {
  dispatch({ type: 'CLEAR_ERRORS' });
};