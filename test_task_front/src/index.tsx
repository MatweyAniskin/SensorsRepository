import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { SensorsProvider } from './components/sensors_provider';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <SensorsProvider>
      <App />
    </SensorsProvider>

  </React.StrictMode>
);

reportWebVitals();
