import { Router, Routes, Route, Link, BrowserRouter } from 'react-router-dom';
import './App.css';
import { siteConfig } from './config/site_config';
import Navigation from './components/navigation';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Navigation/>
        <Routes>
          <Route path={siteConfig.Links.MapPage.href} element={siteConfig.Links.MapPage.component} />
           <Route path={siteConfig.Links.LoadPage.href} element={siteConfig.Links.LoadPage.component} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
