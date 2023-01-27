import React, { useState } from 'react';
import { connect } from 'react-redux';
import { submitLocation } from '../redux/actions/weatherActions';

import '../stylesheets/search.css';

const Search = ({ dispatch, error }) => {
  const [location, setLocation] = useState('');

  const onSumbit = () => {
    dispatch(submitLocation(location));
  };

  return (
    <div>
      <div className="search-container">
        <input
          value={location}
          placeholder="Search Location..."
          onChange={e => setLocation(e.target.value)}
        />
        <button onClick={onSumbit}>Submit</button>
      </div>
      {error ? <p className='error-text'>{error}</p> : null}
    </div>
  )
};

const mapStateToProps = state => ({
  error: state.weather.error
});

export default connect(mapStateToProps)(Search);
