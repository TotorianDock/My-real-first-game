using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private string _dT;
    [SerializeField] private TextMeshProUGUI _dialogueTextDefeat;
    [SerializeField] private TextMeshProUGUI _dialogueTextWon_E;
    [SerializeField] private TextMeshProUGUI _dialogueTextWon_R;
    [SerializeField] private TextMeshProUGUI _dialogueTextWon2;

    private float _textSpeed;
    private float _ExecutionTime;
    private bool _CantInterrupt = false;
    private bool _isGottaSkip = false;
    private bool _isInProgress = false;

    private State _state;
    [SerializeField] private BattleSystem _battleSystem;

    private void Awake()
    {
        if (_battleSystem.state.Equals(BattleState.START))
            if(_battleSystem.isRussianTranslation == true)
                _dialogueText.text = "Д";
            else
                _dialogueText.text = "T";
        else
            _dialogueText.text = string.Empty;
        _dialogueTextDefeat.text = string.Empty;
        _dialogueTextWon_E.text = string.Empty;
        _dialogueTextWon_R.text = string.Empty;
        _dialogueTextWon2.text = string.Empty;
    }
    private IEnumerator StartDialogue(string _line)
    {
        _textSpeed = (_ExecutionTime / 40f) / 1.5f;

        if (_isInProgress)
        {
            _isInProgress = false;
            StopCoroutine(TypeLine(_line));
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(TypeLine(_line));
        }
        else
            StartCoroutine(TypeLine(_line));
    }
    private IEnumerator TypeLine(string _line)
    {
        Awake();
        _isInProgress = true;
        _isGottaSkip = false;
        foreach (char c in _line)
        {
            if (!_isInProgress)
                break;
            if (_isGottaSkip && !_CantInterrupt)
                _textSpeed = 0;
            if (_line.Length > 120)
            {
                _dialogueText.text = _line;
                break;
            }

            switch (_state)
            {
                case State.None:
                    _dialogueText.text += c;
                    break;

                case State.Defeat:
                    _dialogueTextDefeat.text += c;
                    break;

                case State.Win_E:
                    _dialogueTextWon_E.text += c;
                    break;

                case State.Win_R:
                    _dialogueTextWon_R.text += c;
                    break;

                case State.Win2:
                    _dialogueTextWon2.text += c;
                    break;
            }
            yield return new WaitForSeconds(_textSpeed);
        }
        _state = State.None;
        _isInProgress = false;
        _CantInterrupt = false;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.LeftControl) || _isGottaSkip == true)
            _isGottaSkip = true;
    }
    public void EnemyApproaches(Unit enemyUnit, bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            switch (enemyUnit.enemyName)
            {
                case Unit.EnemyName.Snake:
                    _dT = "Дикая Змея подходит...";
                    break;
                
                case Unit.EnemyName.Scorpio:
                    _dT = "Дикий Скорпион подходит...";
                    break;
               
                case Unit.EnemyName.Hyena:
                    System.Random random = new();
                    int randomiser = random.Next(0, 101);
                    if (randomiser == 100)
                        _dT = "Дикая Хиена подходит...";
                    else
                        _dT = "Дикая Гиена подходит...";
                    break;
                
                //case Unit.EnemyName.Vulture:
                //    break;
                
                default:
                    _dT = "The M҈̡̛͔͔̃͒͆̍̏i҈̱̦́̒̾̀̚̚͢͞ś̵̨̬̤̠͓͇͉̮͙͌͐́̽̊̓̚͞s̸̨͕̖͚̝̱͖͗͗̕i̴̢̳͈͕̮̤҇̐́n҈̧̯̜͍̒̅̈́̈́͑̂̓̕ǵ̶̨̮̣̭̭͐͋̐̓̍͠Ë̸͚̦́̈́͌̕͜n̶̨̲͔̩̤̖̎͆̓͆̓̀̓̊͡    approaches.";
                    break;
            }
        }
        else if(!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "The wild Snake approaches...",
                Unit.EnemyName.Scorpio => "The wild Scorpio approaches...",
                Unit.EnemyName.Hyena => "The wild Hyena approaches...",
                //Unit.EnemyName.Vulture => "",
                
                _ => "The M҈̡̛͔͔̃͒͆̍̏i҈̱̦́̒̾̀̚̚͢͞ś̵̨̬̤̠͓͇͉̮͙͌͐́̽̊̓̚͞s̸̨͕̖͚̝̱͖͗͗̕i̴̢̳͈͕̮̤҇̐́n҈̧̯̜͍̒̅̈́̈́͑̂̓̕ǵ̶̨̮̣̭̭͐͋̐̓̍͠Ë̸͚̦́̈́͌̕͜n̶̨̲͔̩̤̖̎͆̓͆̓̀̓̊͡    approaches.",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void ChooseAnAction(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "Выберите действие:";
        else
            _dT = "Choose an action:";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void HeroAttack(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "Вы атакуете...";
        else
            _dT = "You attack ...";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void AttackSuccessful(bool isRussianTranslation, float _ExecutionTime) 
    {
        if (isRussianTranslation)
            _dT = "Ваша атака успешна!";
        else
            _dT = "Your attack is succesful!";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void AttackFailed1(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "... но ваша атака провалена,";
        else
            _dT = "... but your attack failed,";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void AttackFailed2(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "на что вы надеялись?";
        else
            _dT = "what were you hoping for?";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void YouHealed(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "Вы успешно исцелились!";
        else
            _dT = "You succesful healed!";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void YouBlock(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "...но вы блокируете атаку!";
        else
            _dT = "...but you block the attack!";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void YouReady(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "Вы готовитесь заблокировать удар";
        else
            _dT = "You are ready to block the attack";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void EnemyAttackSuccesful(Unit enemyUnit, bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Змея нападает на вас!",
                Unit.EnemyName.Scorpio => "Скорпион нападает на вас!",
                Unit.EnemyName.Hyena => "Гиена нападает на вас!",
                //Unit.EnemyName.Vulture => "",
                
                _ => "The  M҈̱̝̳̟͉̭͕͛̚͜͞i̸̧̱̠̪̊̏̔̒̕ş̷͍̤͍̮̞̦̥͂͌̾̏̀͡ṣ̵̢̭̦̙̫̤̃̿̃̏͑̊͡i̷̢͎̯͔͑̊͡ͅn̵̢͉̦̗̜͂́̅͡ͅg҈̛͎̰̗̫̲̱͋̐̈́̌͑͢E̸̡̮̱͓̔̀̎̄̕ͅͅn̶̢̲͙͓̟͈̫̫̠̿̅̀͠'s attack is successful.",
            };
        }
        else if (!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Snake attacks you!",
                Unit.EnemyName.Scorpio => "Scorpio attacks you!",
                Unit.EnemyName.Hyena => "Hyena attacks you!",
                //Unit.EnemyName.Vulture => "",

                _ => "The  M҈̱̝̳̟͉̭͕͛̚͜͞i̸̧̱̠̪̊̏̔̒̕ş̷͍̤͍̮̞̦̥͂͌̾̏̀͡ṣ̵̢̭̦̙̫̤̃̿̃̏͑̊͡i̷̢͎̯͔͑̊͡ͅn̵̢͉̦̗̜͂́̅͡ͅg҈̛͎̰̗̫̲̱͋̐̈́̌͑͢E̸̡̮̱͓̔̀̎̄̕ͅͅn̶̢̲͙͓̟͈̫̫̠̿̅̀͠'s attack is successful.",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void EnemyAttackFail(Unit enemyUnit, bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Змея нападает на вас...",
                Unit.EnemyName.Scorpio => "Скорпион нападает на вас...",
                Unit.EnemyName.Hyena => "Гиена нападает на вас...",
                //Unit.EnemyName.Vulture => "",

                _ => "The Ḿ̵̧̩̜̱͙̰͒̕i̵̡̠̦̤̦̥̞͇̫̓̓͆̀̑́́͡s̸̨͙̩͉͑̂͛͌̐͡s҉̧͇̯̀̔̎̑̑́͒͠i҈̨̥̮̗̦̱̇̿͌̿̕n̷̢͍͖̬̩̋͋̄͒͊̕ǵ̶̡̟̯̞͚̭̂̇̾̕E̶̩͈̅̇̉͂̚͜͠n҈͖̗̰̝͚̒̓̅́̕͜  attacks на вас...",
            };
        }
        else if (!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Snake attacks you...",
                Unit.EnemyName.Scorpio => "Scorpio attacks you...",
                Unit.EnemyName.Hyena => "Hyena attacks you...",
                //Unit.EnemyName.Vulture => "",

                _ => "The Ḿ̵̧̩̜̱͙̰͒̕i̵̡̠̦̤̦̥̞͇̫̓̓͆̀̑́́͡s̸̨͙̩͉͑̂͛͌̐͡s҉̧͇̯̀̔̎̑̑́͒͠i҈̨̥̮̗̦̱̇̿͌̿̕n̷̢͍͖̬̩̋͋̄͒͊̕ǵ̶̡̟̯̞͚̭̂̇̾̕E̶̩͈̅̇̉͂̚͜͠n҈͖̗̰̝͚̒̓̅́̕͜  attacks на вас...",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void EnemyHealed1(Unit enemyUnit, bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Змея кусает свой хвост...",
                Unit.EnemyName.Scorpio => "Скорпион играет в Доту 2...",
                Unit.EnemyName.Hyena => "Гиена не выступает в цирке...",
                //Unit.EnemyName.Vulture => "",

                _ => "The  M̶̧̪̟̫̗͚̫̪̳҇̂̓͑i̸͔͎͎̠̳͚̋͑̎̋̊̔̄̕͢ͅs҉̪̙̬̠͍͈̲͖̌͊̀̍͐̾̚͜͝s҉̱̱̥̞̱͆̔̅̆͜͝i҉͔̫͔̳̞̉̀̐̑̃̚͢͠n҈̬̙͙̪̉̄͜͞g̵̨̗̠̞͈͔̜͖̱̐̅́̆̕Ḙ̴̢̛͔̰͕̘̝̟̋̽͛͗͋ņ̸̬̮̦̣͒̽̂͗̍̍̾͠  is playing Dota 2...",
            };
        }
        else if (!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Snake biting his own tail...",
                Unit.EnemyName.Scorpio => "Scorpio is playing Dota 2...",
                Unit.EnemyName.Hyena => "Hyena eats carrion...",
                //Unit.EnemyName.Vulture => "",

                _ => "The  M̶̧̪̟̫̗͚̫̪̳҇̂̓͑i̸͔͎͎̠̳͚̋͑̎̋̊̔̄̕͢ͅs҉̪̙̬̠͍͈̲͖̌͊̀̍͐̾̚͜͝s҉̱̱̥̞̱͆̔̅̆͜͝i҉͔̫͔̳̞̉̀̐̑̃̚͢͠n҈̬̙͙̪̉̄͜͞g̵̨̗̠̞͈͔̜͖̱̐̅́̆̕Ḙ̴̢̛͔̰͕̘̝̟̋̽͛͗͋ņ̸̬̮̦̣͒̽̂͗̍̍̾͠  is playing Dota 2...",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void EnemyHealed2(bool isRussianTranslation, Unit enemyUnit, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "... и каким-то образом исцеляется!",
                Unit.EnemyName.Scorpio => "... и каким-то образом выигрывает!",
                Unit.EnemyName.Hyena => "... но она не волк!",
                //Unit.EnemyName.Vulture => "",

                _ => "The M̶̧̪̟̫̗͚̫̪̳҇̂̓͑i̸͔͎͎̠̳͚̋͑̎̋̊̔̄̕͢ͅs҉̪̙̬̠͍͈̲͖̌͊̀̍͐̾̚͜͝s҉̱̱̥̞̱͆̔̅̆͜͝i҉͔̫͔̳̞̉̀̐̑̃̚͢͠n҈̬̙͙̪̉̄͜͞g̵̨̗̠̞͈͔̜͖̱̐̅́̆̕Ḙ̴̢̛͔̰͕̘̝̟̋̽͛͗͋ņ̸̬̮̦̣͒̽̂͗̍̍̾͠  ... но она не волк.",
            };
        }
        else if (!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "... and somehow gets healed!",
                Unit.EnemyName.Scorpio => "... and somehow wins!",
                Unit.EnemyName.Hyena => "... and he heals!",
                //Unit.EnemyName.Vulture => "",

                _ => "The M̶̧̪̟̫̗͚̫̪̳҇̂̓͑i̸͔͎͎̠̳͚̋͑̎̋̊̔̄̕͢ͅs҉̪̙̬̠͍͈̲͖̌͊̀̍͐̾̚͜͝s҉̱̱̥̞̱͆̔̅̆͜͝i҉͔̫͔̳̞̉̀̐̑̃̚͢͠n҈̬̙͙̪̉̄͜͞g̵̨̗̠̞͈͔̜͖̱̐̅́̆̕Ḙ̴̢̛͔̰͕̘̝̟̋̽͛͗͋ņ̸̬̮̦̣͒̽̂͗̍̍̾͠   ... но она не волк.",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void EnemyReadyToBlock1(Unit enemyUnit, bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Змея сворачивается в уробороса,",
                Unit.EnemyName.Scorpio => "Скорпион становиться BALL,",
                Unit.EnemyName.Hyena => "Гиена смеется над вами",
                //Unit.EnemyName.Vulture => "",

                _ => "The M̶̧̪̟̫̗͚̫̪̳҇̂̓͑i̸͔͎͎̠̳͚̋͑̎̋̊̔̄̕͢ͅs҉̪̙̬̠͍͈̲͖̌͊̀̍͐̾̚͜͝s҉̱̱̥̞̱͆̔̅̆͜͝i҉͔̫͔̳̞̉̀̐̑̃̚͢͠n҈̬̙͙̪̉̄͜͞g̵̨̗̠̞͈͔̜͖̱̐̅́̆̕Ḙ̴̢̛͔̰͕̘̝̟̋̽͛͗͋ņ̸̬̮̦̣͒̽̂͗̍̍̾͠  curls up in a BALL,",
            };
        }
        else if (!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Snake curls up into a uroboros",
                Unit.EnemyName.Scorpio => "Scorpio curls up in a BALL,",
                Unit.EnemyName.Hyena => "Hyena's laughing at you",
                //Unit.EnemyName.Vulture => "",

                _ => "The M̶̧̪̟̫̗͚̫̪̳҇̂̓͑i̸͔͎͎̠̳͚̋͑̎̋̊̔̄̕͢ͅs҉̪̙̬̠͍͈̲͖̌͊̀̍͐̾̚͜͝s҉̱̱̥̞̱͆̔̅̆͜͝i҉͔̫͔̳̞̉̀̐̑̃̚͢͠n҈̬̙͙̪̉̄͜͞g̵̨̗̠̞͈͔̜͖̱̐̅́̆̕Ḙ̴̢̛͔̰͕̘̝̟̋̽͛͗͋ņ̸̬̮̦̣͒̽̂͗̍̍̾͠  curls up in a BALL,",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void EnemyReadyToBlock2(bool isRussianTranslation, Unit enemyUnit, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "чтобы заблокировать вашу атаку?",
                Unit.EnemyName.Scorpio => "чтобы заблокировать вашу атаку?",
                Unit.EnemyName.Hyena => "Вы смущаетесь, вы можете промануться",
                //Unit.EnemyName.Vulture => "",

                _ => "You're shy, you might miss",
            };
        }
        else if (!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "to block your attack?",
                Unit.EnemyName.Scorpio => "to block your attack?",
                Unit.EnemyName.Hyena => "You're shy, you might miss",
                //Unit.EnemyName.Vulture => "",

                _ => "Вы смущаетесь, вы можете промануться",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void EnemyIsDead(Unit enemyUnit, bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "Дикая Змея мертва!",
                Unit.EnemyName.Scorpio => "Дикий Скорпион мертв!",
                Unit.EnemyName.Hyena => "Дикая Гиена мертва!",
                //Unit.EnemyName.Vulture => "",

                _ => "The M̵̡͕̱̒̆́̿̕i҈̳̩͈̓̽̓͊̕͜s̴̡̞̱̫̭͎̣͉̆̈̆͞s̶̨̗̝̬̤̮̪̟͇̆̌̆̕i̵͖̠̫̭̊̆͌͢͝n҉̡̙̫̜̞̟̩̤̖͆́̄̌͡g҈̨̠͎̝̝̋̀͌̋̓̆͛͠ͅE҈̧̛̬͓͖̰͋͌͛̎͆͗͌n҈̠̞҇̆̐͜  is dead.?",
            };
        }
        else if (!isRussianTranslation)
        {
            _dT = enemyUnit.enemyName switch
            {
                Unit.EnemyName.Snake => "The wild Snake is dead!",
                Unit.EnemyName.Scorpio => "The wild Scorpio is dead!",
                Unit.EnemyName.Hyena => "The wild Hyena is dead!",
                //Unit.EnemyName.Vulture => "",

                _ => "The M̵̡͕̱̒̆́̿̕i҈̳̩͈̓̽̓͊̕͜s̴̡̞̱̫̭͎̣͉̆̈̆͞s̶̨̗̝̬̤̮̪̟͇̆̌̆̕i̵͖̠̫̭̊̆͌͢͝n҉̡̙̫̜̞̟̩̤̖͆́̄̌͡g҈̨̠͎̝̝̋̀͌̋̓̆͛͠ͅE҈̧̛̬͓͖̰͋͌͛̎͆͗͌n҈̠̞҇̆̐͜  is dead.?",
            };
        }

        this._ExecutionTime = _ExecutionTime;
        _CantInterrupt = true;
        StartCoroutine(StartDialogue(_dT));
    }
    public void LocationChange(bool isRussianTranslation, float _ExecutionTime)
    {
        _dialogueTextWon_E.text = string.Empty;

        if (isRussianTranslation)
            _dT = "Смена локации...";
        else
            _dT = "Changing location...";

        this._ExecutionTime = _ExecutionTime;
        _CantInterrupt = true;
        StartCoroutine(StartDialogue(_dT));
    }
    public void YouIsDead(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "Ваш герой мертв.";
        else
            _dT = "Your hero is dead.";

        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void YouLost(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "Вы были побеждены.";
        else
            _dT = "You were defeated.";
        
        _state = State.Defeat;
        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void YouWonBattle(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
        {
            _dT = "Вы выиграли битву!";
            _state = State.Win_R;
        }
        else
        {
            _dT = "You won the battle!";
            _state = State.Win_E;
        }

        _CantInterrupt = true;
        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }
    public void YouWonGame(bool isRussianTranslation, float _ExecutionTime)
    {
        if (isRussianTranslation)
            _dT = "Вы выиграли игру!";
        else
            _dT = "You won the game!";

        _state = State.Win2;
        this._ExecutionTime = _ExecutionTime;
        StartCoroutine(StartDialogue(_dT));
    }

    private enum State
    {
        None = 0,
        Win_E = 1,
        Win_R = 2,
        Win2 = 3,
        Defeat = 4
    }
}
