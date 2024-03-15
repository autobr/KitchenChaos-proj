using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    // Constants: UPPER_CASE / snake_case
    public const int CONSTANT_FIELD = 56;

    // Properties = PascalCase
    public static ExampleScript Instance { get; private set; }

    // Fields: camelCase (iLoveCamelCase)
    private float memberVariable;

    // Function Names: PascalCase
    private void Awake()
    {
        Instance = this;

        DoSomething(10f);
    }

    // Function Params: camelCase
    private void DoSomething(float time)
    {
        // Do something...
        memberVariable = time + Time.deltaTime;
        if (memberVariable > 0)
        {
            // Do something else I guess...
        }
    }

    // kebab-case-also-exists

    private string strName = "Hungarian Notation contains the type of variable within the name + camelCase";

    // AND_SCREAMING_SNAKE_CASE_FOR_IF_YOUR_SNAKE_IS_LOUD
}