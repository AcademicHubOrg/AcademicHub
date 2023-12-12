import React, {useState} from 'react';
import { addCourseStream } from '../../api/addCourseStreamService';
import { useMyContext } from '../../MyContext';

const CreateCoursePage: React.FC = () => {
    const { jsonData} = useMyContext();
    const templateId = jsonData.templateIDJSON;
    const [inputText, setInputText] = useState('');
    const [displayedText, setDisplayedText] = useState('');

    const handleButtonClick = () => {
        addCourseStream(inputText, templateId as string) // Send the POST request
            .catch(error => console.error('POST request failed: ', error));
        setDisplayedText(`Course "${inputText}" created`);
    };

    return (
        <div>
            <h1>Create Course Page From Template {templateId}</h1>
            <input
                type="text"
                value={inputText}
                onChange={(e) => setInputText(e.target.value)}
                placeholder="Course Stream Name"
            />
            <button onClick={handleButtonClick}>Display Text</button>
            {displayedText &&<p>{displayedText}</p>}
        </div>

    );
};

export default CreateCoursePage;
