import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './TopMenu.css'; // Assuming you have a CSS file for styles

const TopMenu: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const isActive = (path: string) => location.pathname === path;

    return (
        <div className="topMenu">
            <button className={isActive('/') ? 'active' : ''} onClick={() => navigate('/')}>Home</button>
            <button className={isActive('/profile') ? 'active' : ''} onClick={() => navigate('/profile')}>Profile Page</button>
            <button className={isActive('/courselist') ? 'active' : ''} onClick={() => navigate('/courselist')}>Courses stream List</button>
            <button className={isActive('/templateList') ? 'active' : ''} onClick={() => navigate('/templateList')}>Courses template List</button>
            <button className={isActive('/template') ? 'active' : ''} onClick={() => navigate('/template')}>Add Template</button>
            <button className={isActive('/auth') ? 'active' : ''} onClick={() => navigate('/auth')}>Auth0</button>

        </div>
    );
};

export default TopMenu;

