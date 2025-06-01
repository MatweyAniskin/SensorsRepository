import React from 'react';
import logo from './logo.svg';
import './App.css';
import SensorsMap from './components/map';
import { useSensors } from './components/sensors_provider';
function App() {
  const {sensors} = useSensors()
  return (
    <div className="App">
      <SensorsMap sensors={sensors}/>
    </div>
  );
}

export default App;
