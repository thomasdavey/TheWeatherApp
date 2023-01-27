const initialState = {
  isLoading: false,
  weather: null,
  error: null
};

const weatherReducer = (state = initialState, action) => {
  switch (action.type) {
    case 'WEATHER_LOADING':
      return {
        ...state,
        isLoading: true
      };
    case 'WEATHER_LOADED':
      return {
        ...state,
        isLoading: false,
        weather: action.weather,
        error: null
      };
    case 'WEATHER_ERROR':
      return {
        ...state,
        isLoading: false,
        weather: null,
        error: action.error
      };
    case 'CLEAR_ERRORS':
      return {
        ...state,
        error: null
      };
    default:
      return state;
  }
};

export default weatherReducer;