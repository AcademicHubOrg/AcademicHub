// MyContext.tsx
import React, { createContext, useContext } from 'react';

interface MyContextProps {
    jsonData: any; // You can replace 'any' with a more specific type if needed
    updateJsonData: (newJsonData: any) => void;
}

const MyContext = createContext<MyContextProps | undefined>(undefined);

interface MyProviderProps {
    children: React.ReactNode;
}

export const MyProvider: React.FC<MyProviderProps> = ({ children }) => {
    const [jsonData, setJsonData] = React.useState<any>({});

    const updateJsonData = (newJsonData: any) => {
        setJsonData(newJsonData);
    };

    return (
        <MyContext.Provider value={{ jsonData, updateJsonData }}>
            {children}
        </MyContext.Provider>
    );
};

export const useMyContext = () => {
    const context = useContext(MyContext);
    if (!context) {
        throw new Error('useMyContext must be used within a MyProvider');
    }
    return context;
};
