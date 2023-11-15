import React from 'react';
import { useNavigate } from 'react-router-dom';

const TopMenu: React.FC = () => {
    const navigate = useNavigate();

    return (
        <div>
            <button onClick={() => navigate('/')}>Home</button>
            <button onClick={() => navigate('/courselist')}>Courses List</button>
            <button onClick={() => navigate('/auth')}>Auth</button>
            <button onClick={() => navigate('/addcourse')}>Add Course</button>
            <button onClick={() => navigate('/template')}>Template</button>
        </div>
    );
};

export default TopMenu;
