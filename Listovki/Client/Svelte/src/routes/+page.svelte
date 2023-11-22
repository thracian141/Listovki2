<script lang="ts">
    import { browser } from '$app/environment';
    import './styles.css'
	import { slide } from 'svelte/transition';
    import { fly } from 'svelte/transition';
    let playbutton = 'src/lib/img/playButton.svg';
    let playbuttonHover = 'src/lib/img/playButtonHover.svg';
    let lolCursor = 'src/lib/img/cursor/normal.cur';
    let audio : HTMLAudioElement | null = null;
    let audio2 : HTMLAudioElement | null = null;
    if (browser) {
        audio = new Audio('src/lib/sound/hoverButtonSound.mp3');
        audio2 = new Audio('src/lib/sound/PlayAcceptSound.mp3');

        audio.load();
        audio2.load();
    }

    function playSound2() {
        stopSound();
        if (browser && audio2) {
            audio2.play();
        }
    }

    function playSound() {
        if (browser && audio) {
            audio.play();
        }
    }
    function stopSound() {
        if (browser && audio) {
            audio.pause();
            audio.currentTime = 0;
        }
    }

    let motorbike = 'src/lib/img/koli/motorbike.png';
    let car = 'src/lib/img/koli/car.png';
    let truck = 'src/lib/img/koli/truck.png';
    let bus = 'src/lib/img/koli/bus.png';

    let src = motorbike;
    let label = 'A категория';
    let category = 1;
    $: direction = 1;

    function changeCategory(increment : number, event: Event) {
        event.preventDefault();
        direction = increment;
        category += increment;
        if (category > 4) {
            category = 1;
        } else if (category < 1) {
            category = 4;
        }
        switch (category) {
            case 1:
                src = motorbike;
                label = 'А категория';
                selectedCategory = 'A';
                break;
            case 2:
                src = car;
                label = 'B категория';
                selectedCategory = 'B';
                break;
            case 3:
                src = truck;
                label = 'С категория';
                selectedCategory = 'C';
                break;
            case 4:
                src = bus;
                label = 'D категория';
                selectedCategory = 'D';
                break;
            default:
                src = car;
                label = 'B категория';
                selectedCategory = 'B';
                break;
        }
    }  
    
    let selectedCategory : string = 'A';

    function selectCategory(catg : string) {
        selectedCategory = catg;
        switch (catg) {
            case 'A':
                src = motorbike;
                label = 'А категория';
                category = 1;
                break;
            case 'B':
                src = car;
                label = 'B категория';
                category = 2;
                break;
            case 'C':
                src = truck;
                label = 'С категория';
                category = 3;
                break;
            case 'D':
                src = bus;
                label = 'D категория';
                category = 4;
                break;
            default:
                src = car;
                label = 'B категория';
                category = 2;
                break;
        }
    }
</script>

<div class="wrapper" style="margin-top: 1rem; width:80%">
    <h1>Добре дошли в Листовки!</h1>
</div>
<div style="display: flex; flex-direction:row; width: 80%;">
    <div class="wrapper" style="width:55%;">
        <div style="display:flex; flex-direction:row; width:100%; justify-content:center; gap:4rem;">
            <div style="display:flex;flex-direction:column; margin-top:4rem; width:20%;">
                <div style="display: flex; flex-direction:row; gap:2rem; margin-bottom: 3.5rem;">
                    <a href="/" class="categoryOption" class:categorySelected={selectedCategory === 'A'} on:click={() => selectCategory('A')}><img src={motorbike} alt="A category"/><p>A</p></a>
                    <a href="/" class="categoryOption" class:categorySelected={selectedCategory === 'B'} on:click={() => selectCategory('B')}><img src={car} alt="A category"/><p>B</p></a>
                </div>
                <div style="display: flex; flex-direction:row; gap:2rem;">
                    <a href="/" class="categoryOption" class:categorySelected={selectedCategory === 'C'} on:click={() => selectCategory('C')}><img src={truck} alt="A category"/><p>C</p></a>
                    <a href="/" class="categoryOption" class:categorySelected={selectedCategory === 'D'} on:click={() => selectCategory('D')}><img src={bus} alt="A category"/><p>D</p></a>
                </div>
            </div>
            <div style="display:flex; flex-direction:column; text-align:center; width:60%; align-items:center; justify-content:center;">
                <h2>Решавай листовка:</h2>
                <div id="categoryWrapper">
                    <a id="scrollright" style="right:-20%;" href="/" on:click={(event) => changeCategory(1, event)}>
                        <img src="src/lib/img/chevron-right.svg" alt="scroll right"/>
                    </a>
                    <a id="scrollleft" style="left:-20%;" href="/" on:click={(event) => changeCategory((-1), event)}>
                        <img src="src/lib/img/chevron-left.svg" alt="scroll left"/>
                    </a>   
                    <div id="categoryPicker"> 
                        {#each [src] as imageSrc (imageSrc)}
                            <img src={imageSrc} alt="vehicle" in:fly={{ y: 0, x: direction * 100, duration: 500 }} out:fly={{ y: 0, x: -direction * 100, duration: 500 }} class="thumbnail"/>
                        {/each}
                        <span class="text-white" style="position:absolute; font-size:2rem; bottom:0%; right:5%;">{label}</span>    
                    </div>
                </div>
            </div>
            <div style="display: flex; flex-direction:column; width:20%; margin-top:4.5rem; position:relative">
                <img id="info-icon" src="src/lib/img/info.png" alt="info" style="cursor: url('src/lib/img/cursor/normal.cur'),auto"/>
                <div id="information" style="position: absolute; top:75%; left:50%; transform:translateX(-50%)">
                    <h3 class="text-white" style="margin:none; text-shadow:0px 1px 2px #000000ff; font-weight:bold;">Информация:</h3>
                    <p class="text-dark" style="margin:none; text-shadow:none;">Имате 40 минути да отговорите на всички въпроси.
                        За всеки въпрос има поне един верен отговор,
                        като броят верни е посочен допълнително до въпроса.
                    </p>
                </div>
            </div>
        </div>
        <a href="/listovki/{selectedCategory}" id="playButton" on:click={playSound2}
         on:mouseover="{playSound}" on:mouseout="{stopSound}" on:blur="{stopSound}" on:focus="{playSound}"
         style="cursor: url('src/lib/img/cursor/link.cur'),auto; margin-top:1rem;"><img src={playbutton} alt="begin exam" /><p>ЗАПОЧНИ</p></a>
    </div>
    <div class="wrapper" style="width:45%">
        <div id="checkresults">
            <div id="checkresultsUp">
                <h1>Провери резултат:</h1>
            </div>
            <div style="display: flex; flex-direction:row; width:100%; height:30%; justify-content:center; align-items:center;">
                <div style="width: fit-content !important; display:flex; border-radius:1rem; outline: #6c757d55 solid 1px; padding:2px;">
                    <div style="display: flex; flex-direction:row; position:relative; 
                       width:24rem; background-color:#28a74555; border-radius:1rem; padding:0.5rem;">
                        <input placeholder="Номер на листовка" id="input-code" type="number"/>
                        <button class="btn btn-primary" id="input-code-button">Въведи</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    #input-code-button {
        display: flex;
        position: absolute;
        right:2.5%;
        top:50%;
        transform: translateY(-50%);
        font-size: 1.3rem;
        font-weight:normal;
        border-radius: 1rem;
        height: 3rem;
        line-height: 2.1rem;
        width:6rem;
        cursor: pointer;
    }
    #input-code {
        font-size: 1.4rem;
        border-radius: 0.8rem;
        height: 3rem;
        width:16rem;
        border:none;
        transition: box-shadow 0.2s ease-in-out;
        appearance: none;
        -webkit-appearance: none;
        -moz-appearance: textfield;
        padding:0.5rem;
        box-shadow: 0px 0px 5px 0px #00000055;
        transition: box-shadow 0.2s ease-in-out;
    }
    #input-code::-webkit-inner-spin-button,
    #input-code::-webkit-outer-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }
    #input-code:focus {
        outline: none;
        box-shadow: 0px 0px 7px 0px #007bff;
    }
    #input-code::placeholder {
        color: #c5c5c8;
    }
    #checkresults {
        display: flex;
        flex-direction: column;
        width: 100%;
        height: 100%;
        background-color: rgba(255,255,255,0.5);
        border-radius: 1rem;
        overflow:hidden;
    }
    #checkresultsUp {
        display: flex;
        justify-content: center;
        align-items: flex-end;
        width:100%;
        height:70%;
        background-image: url('/resultsbg.jpg');
        background-position-y: 20%;
        background-size: cover;
        background-repeat: none;
    }
    #information {
        position: absolute;
        height:0rem;
        overflow: hidden;
        background-color: rgba(255, 255, 255, 0.6);
        padding-left:1rem; padding-right: 1rem;
        border-radius: 0.5rem;
        transition: height 0.2s ease-in-out, padding-top 0.2s ease-in-out;
    }
    #info-icon:hover + #information {
        padding-top: 0.5rem;
        height:18rem;
        object-fit: cover;
        image-rendering: optimizeQuality;
        display: block;
    }
    .categoryOption {
        display: flex;
        flex-direction: column;
        position: relative;
        justify-content: center;
        align-items: center;
        filter: grayscale(100%);
        border-radius: 100%;
        width:4rem;
        height:4rem;
        background-color: rgba(255,255,255,0.4);
        text-decoration: none;
        text-shadow: none;
        color:#6c757d;
        outline: #6c757d68 solid 0.2rem;
        text-align: center;
        overflow:visible;
        font-size: 1.5rem;
        transition: filter,box-shadow 0.2s ease-in-out;
        cursor: pointer;
    }
    .categoryOption:hover {
        filter:none;
        box-shadow: 0px 0px 12px 0px #6c757d;
    }
    .categorySelected {
        filter: none;
        outline-color: #007bff !important;
        color: #007bff !important;
        box-shadow: 0px 0px 10px 0px #007bff;
    }
    .categoryOption > p {
        position: absolute;
        top: 100%;
        margin-bottom:none;
    }
    .categoryOption > img {
        width:5rem;
        height:auto;
        object-fit: cover;
        z-index: 999;
    }
    #playButton {
        display: flex;
        position: relative;
    }
    #playButton > img {
        transition: filter 0.2s ease-in-out;
    }
    #playButton:hover > img {
        filter: drop-shadow(0px 0px 3px rgb(0 123 255 / 0.7));
    }
    #playButton > p {
        position:absolute;
        text-decoration: none;
        color:white;
        font-family: 'lolFont';
        font-size: 1.8rem;
        top:50%;
        left:60%;
        transform: translate(-50%, -50%);
        transition: opacity 0.2s ease-in-out;
    }
    #playButton:hover > p {
        opacity: 0.6;
    }
    .wrapper > h1 {
        font-size: 3rem;
    }
    .wrapper > a > img {
        width:20rem;
        height:auto;
        margin:0;
        image-rendering: optimizeQuality;
    }

    #categoryWrapper {
        position: relative;
        display:flex;
        flex-direction: row;
        justify-content: center;
        align-items: center;
        width: 16rem;
        height: 15rem;
    }
    #categoryPicker {
        position: relative;
        display:flex;
        flex-direction: column;
        background-color: rgba(255,255,255,0.6);
        border-radius: 1rem;
        padding: 1rem;
        width: 100%; height:100%;
        overflow: hidden;
    }
    #categoryWrapper > a {
        position: absolute;
        top: 50%;
        transform: translateY(-50%);
        cursor: pointer;
    }
    #categoryWrapper > a > img {
        filter: brightness(0) saturate(100%) invert(100%) sepia(0%) saturate(1%) hue-rotate(89deg) brightness(101%) contrast(101%) 
            drop-shadow(0px 0px 3px rgb(0 0 0 / 0.4));
        transition: filter 0.2s ease-in-out;
        width: 3.5rem;
        margin:0;
        height:auto;
    }
    #categoryWrapper > a > img:hover {
        filter: brightness(0) saturate(100%) invert(100%) sepia(0%) saturate(1%) hue-rotate(89deg) brightness(101%) contrast(101%) 
            drop-shadow(0px 0px 6px rgb(0 123 255 / 0.7));
    }
    .thumbnail {
        height: 12rem;
        width: 14rem;
        image-rendering: optimizeQuality;
        object-position: left;
        object-fit: cover;
        position: absolute;
    }
    @font-face {
        font-family: 'lolFont';
        src: url('../lib/fonts/BeaufortforLOL-Bold.woff2'),
             url('../lib/fonts/BeaufortforLOL-Bold.ttf');
    }
    #categoryPicker > span {
        font-family: 'lolFont';
        text-shadow: 0px 0px 3px rgba(0,0,0, 0.65);
    }
</style>