import React from 'react';
import Search from './search';
import { connect } from 'react-redux';
import { clearErrors } from '../redux/actions/weatherActions';
import { FiSunset, FiSunrise } from 'react-icons/fi'
import { HiOutlineArrowNarrowUp, HiOutlineArrowNarrowDown } from 'react-icons/hi';
import { MdLocationPin } from 'react-icons/md';

import '../stylesheets/weather.css';

const Weather = ({ dispatch, weather, error }) => {
  React.useEffect(() => {
    dispatch(clearErrors());
  }, [dispatch]);

  return (
    <div className='container'>
      <Search />
      {(weather) &&
        <div className='weather-main'>
          <div className='location'>
            <MdLocationPin />
            <p>{weather.Location.Name}, {weather.Location.Country}</p>
          </div>
          <h1 className='current-temp'>{Math.round(weather.Temperature.Current)}°C</h1>
          <div className='high-low-temp'>
            <div className='low'>
              <HiOutlineArrowNarrowDown />
              <p>{Math.round(weather.Temperature.Minimum)}°C</p>
            </div>
            <div className='high'>
              <HiOutlineArrowNarrowUp />
              <p>{Math.round(weather.Temperature.Maximum)}°C</p>
            </div>
          </div>
          <div className='conditions'>
            <img src={`http://openweathermap.org/img/wn/${weather.Icon}@2x.png`} alt={`${weather.Conditions} weather icon`} />
            <p>{weather.Conditions}</p>
          </div>
          <div className='extra-info'>
            <div className='sunrise-sunset'>
              <div className='sunrise'>
                <FiSunrise />
                <p>{weather.Sunrise}</p>
              </div>
              <div className='sunset'>
                <FiSunset />
                <p>{weather.Sunset}</p>
              </div>
            </div>
            <p className='extra-small'>Pressure</p>
            <p>{weather.Pressure} hPa</p>
            <p className='extra-small'>Humiditiy</p>
            <p>{weather.Humidity}%</p>
            <p className='extra-small'>Feels Like</p>
            <p>{Math.round(weather.Temperature.Maximum)}°C</p>
          </div>
        </div>
      }
    </div>
  );
};

const mapStateToProps = state => ({
  weather: state.weather.weather,
  error: state.weather.error
});

export default connect(mapStateToProps)(Weather);
