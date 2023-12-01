import React from 'react';
import {useParams} from "react-router-dom";

const CreateCoursePage: React.FC = () => {
    const { templateId } = useParams<{ templateId: string }>();

    return (
        <div>Create Course Page {templateId}</div>
    );
};

export default CreateCoursePage;
