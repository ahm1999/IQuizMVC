console.log("this is quiz player")
let url1 = window.location.href;
url1 = url1.replace("/play","")

fetch(url1, {
    method: "get"
}).then(
    res => {

        return res.json()
    }
).then(function (data) {
    // `data` is the parsed version of the JSON returned from the above endpoint.
    //console.log(data);  // { "userId": 1, "id": 1, "title": "...", "body": "..." }
   return playQuiz(data)
});


async function playQuiz(data) {
    arr = []

    arr = data.questions

    i = 0
     displayQuesiton(arr[i])

    $("#nextBtn").on("click", () => {
        ++i
        $("#answer").text("")
        displayQuesiton(arr[i])
    })
   
    
}


function displayQuesiton(question) {
    $("#question").text(question.content)

    $("#revealBtn").on("mouseenter",() => {
        $("#answer").text(question.answer)
    })
 
}

const delay = ms => new Promise(res => setTimeout(res, ms));
