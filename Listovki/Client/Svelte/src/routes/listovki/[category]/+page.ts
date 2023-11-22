export const load = ({ fetch, params}) => {
    const fetchTest = async () => {
        const data = params.category;
        return data;
    }
    
    return {
        test: fetchTest()
    }
}